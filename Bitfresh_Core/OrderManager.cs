using BittrexSharp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bitfresh_Core
{
    public class OrderManager : IDisposable
    {
        private bool disposed, fFirstDataRetrieval;
        public bool fStop;
        public bool fRestoreBackup, fDeleteBackup;
        public BittrexBridge Bridge;
        private Task ListUpdater;

        private Task BackupTask;
        private Stack<OpenOrder> ToBackup;
        private List<string> ToErase;

        private List<Task> TaskingList;
        private IStatus Status;
        public BackupUtility bkp;
        private CancellationTokenSource cancelAwait;

        private readonly int _age;
        private readonly int _frecuency;

        public OrderManager(BittrexBridge bridge, IStatus status, int age, int frecuency)
        {
            _age = age;
            _frecuency = frecuency;
            Bridge = bridge;
            disposed = false;
            fStop = false;
            Status = status;
            fFirstDataRetrieval = fRestoreBackup = fDeleteBackup = false;
            ToBackup = new Stack<OpenOrder>();
            ToErase = new List<string>();

            cancelAwait = new CancellationTokenSource();

            bkp = new BackupUtility(Bridge.apiKeyStore);

            TaskingList = new List<Task>();

            ListUpdater = new Task(new Action(orderListUpdaterCode));

            BackupTask = new Task(new Action(backupOrders));
        }

        private async void orderListUpdaterCode()
        {
            while (!fStop)
            {
                if (!Bridge.fConnected)
                {
                    await Task.Delay(500);
                    continue;
                }

                TaskingList.RemoveAll(x => x.IsCompleted); //clear completed tasks

                Status.STATUS = "Trying to retrieve new data";

                if (!await Bridge.getCurrentsOrders())
                {
                    Status.STATUS = "Failed to retrieve current order data";
                    await Task.Delay(10 * Constants.second);
                    continue;
                }

                fFirstDataRetrieval = true;

                foreach (OpenOrder it in Bridge.ActiveOrders)
                {
                    bool fNotCancelling = string.IsNullOrEmpty(Bridge.cancelling.Find(x => x == it.OrderUuid));

                    TimeSpan time = it.Opened.AddDays(Constants.MaxDays) - DateTime.UtcNow;
#if DEBUG
                    if (time < new TimeSpan(27, 23, 55, 0) && fNotCancelling) //Every 5 Minutes
#else
                    if (time < new TimeSpan(Constants.MaxDays - _age, 0, 0, 0) && fNotCancelling)
#endif
                    {
                        Task TempTask = new Task(() => remakeOrder(it));
                        TaskingList.Add(TempTask);
                        TempTask.Start();
                    }
                }

                Status.STATUS = "Data retrieved";

                try
                {
#if DEBUG
                    await Task.Delay(2 * Constants.minute, cancelAwait.Token); //Every 2 minutes
#else
                    await Task.Delay(_frecuency * Constants.hour, cancelAwait.Token); //HOURS
#endif
                }
                catch (TaskCanceledException)
                {
                    await Task.Delay(10 * Constants.second);
                }
            }
        }

        private void remakeOrder(OpenOrder order, bool fOnlyRecreate = false)
        {
            if (!fOnlyRecreate)
            {
                Status.setStatusByOrder(order.OrderUuid, "Cancelling...");

                if (!Bridge.cancelOrder(order.OrderUuid).Result)
                {
                    Status.setStatusByOrder(order.OrderUuid, "Failed to Cancel");
                    return;
                }

                ToBackup.Push(order);

                Status.setStatusByOrder(order.OrderUuid, "Canceled");
            }

            System.Threading.Thread.Sleep(1000);

            Status.setStatusByOrder(order.OrderUuid, "Recreating...");

            if (!Bridge.createOrder(order, cancelAwait).Result)
            {
                Status.setStatusByOrder(order.OrderUuid, "Unknown order");
                return;
            }

            ToErase.Add(order.OrderUuid);

            Status.removeStatusByOrder(order.OrderUuid);

            return;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (!Bridge.fConnected)
                {
                    Bridge.fFailedConection = true;
                }

                foreach (Task t in TaskingList)
                {
                    t.Wait();
                }

                fStop = true;

                if (BackupTask.Status == TaskStatus.Running)
                {
                    BackupTask.Wait();
                }

                TaskingList.Clear();
                bkp.Dispose();
            }
            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public bool CheckifTask()
        {
            return TaskingList.Any();
        }

        public void startTasks()
        {
            BackupTask.Start();
            ListUpdater.Start();
            return;
        }

        public void backupOrders()
        {
            int counter = 0;

            while (!fStop)
            {
                if (fRestoreBackup && fFirstDataRetrieval)
                {
                    List<OpenOrder> auxList = bkp.Get();

                    Bridge.OnHoldOrders.AddRange(auxList.Where(x => !Bridge.ActiveOrders.Exists(y => (y.Exchange == x.Exchange && y.Limit == x.Limit && y.OrderType == x.OrderType && y.Quantity == x.QuantityRemaining))).ToList());

                    Parallel.ForEach(Bridge.OnHoldOrders, (it) =>
                    {
                        Task TempTask = new Task(() => remakeOrder(it, true));
                        TaskingList.Add(TempTask);
                        TempTask.Start();
                    });

                    foreach (OpenOrder it in auxList)
                    {
                        ToErase.Add(it.OrderUuid);
                    }

                    fRestoreBackup = false;
                }

                if (fDeleteBackup)
                {
                    List<OpenOrder> auxList = bkp.Get();

                    foreach (OpenOrder it in auxList)
                    {
                        ToErase.Add(it.OrderUuid);
                    }
                    fDeleteBackup = false;
                }

                if (ToBackup.Any())
                {
                    if (!Bridge.fConnected)
                    {
                        System.Threading.Thread.Sleep(500);
                        continue;
                    }

                    OpenOrder auxOrder = ToBackup.Pop();

                    if (!ToErase.Contains(auxOrder.OrderUuid))
                    {
                        bkp.writeOrder(auxOrder);
                    }
                }

                if (counter > 120)
                {
                    if (ToErase.Any())
                    {
                        bkp.eraseOrder(ToErase);
                    }
                    counter = 0;
                }
                else
                {
                    counter++;
                }

                System.Threading.Thread.Sleep(500);
            }

            //Erase at exit

            if (ToErase.Any())
            {
                bkp.eraseOrder(ToErase);
            }
        }
    }
}