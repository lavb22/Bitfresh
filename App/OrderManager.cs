using BittrexSharp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bitfresh
{
    public class OrderManager : IDisposable
    {
        private bool disposed, fFirstDataRetrieval;
        public bool fStop;
        public bool fRestoreBackup, fDeleteBackup;
        private BittrexBridge Bridge;
        private Task ListUpdater;

        private Task GUIupdater;

        private Task BackupTask;
        private Stack<OpenOrder> ToBackup;
        private List<string> ToErase;

        private List<Task> TaskingList;
        private statusClass Status;
        private BackupUtility bkp;
        private System.Collections.ObjectModel.ObservableCollection<Order> OrderList;
        private CancellationTokenSource cancelAwait;

        public OrderManager(BittrexBridge bridge, System.Collections.ObjectModel.ObservableCollection<Order> orderList, statusClass status)
        {
            Bridge = bridge;
            disposed = false;
            fStop = false;
            Status = status;
            OrderList = orderList;
            fFirstDataRetrieval = fRestoreBackup = fDeleteBackup = false;
            ToBackup = new Stack<OpenOrder>();
            ToErase = new List<string>();

            cancelAwait = new CancellationTokenSource();

            bkp = new BackupUtility(Bridge.apiKeyStore);

            TaskingList = new List<Task>();

            ListUpdater = new Task(new Action(orderListUpdaterCode));

            GUIupdater = new Task(new Action(UpdateGUI));
            GUIupdater.Start();

            BackupTask = new Task(new Action(backupOrders));
        }

        private void UpdateGUI()
        {
            List<string> tempIDs = new List<string>();
            List<Order> tempData = new List<Order>();

            bool fonlyOnce = true;
            while (!fStop)
            {
                if (Bridge.fFailedConection)
                {
                    fStop = true;

                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        (System.Windows.Application.Current.MainWindow as MainWindow).ShowConnectionError();
                    });
                }

                if (!Bridge.fConnected)
                {
                    System.Threading.Thread.Sleep(500);
                    continue;
                }

                if (fonlyOnce)
                {
                    if (bkp.ordersToRestore())
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            (System.Windows.Application.Current.MainWindow as MainWindow).AskForBackup();
                        });
                    }
                    else
                    {
                        BackupTask.Start();
                        ListUpdater.Start();
                    }
                    fonlyOnce = false;
                }

                tempData = OrderList.Where(el => !(Bridge.OnHoldOrders.Exists(x => x.OrderUuid == el.ID)||Bridge.ActiveOrders.Exists(x => x.OrderUuid == el.ID))).ToList();

                if (tempData.Any())
                {

                    foreach (Order it in tempData)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            OrderList.Remove(it);
                        });

                    }

                }

                tempData = OrderList.Where(el => Bridge.OnHoldOrders.Exists(x => x.OrderUuid == el.ID) || Bridge.ActiveOrders.Exists(x => x.OrderUuid == el.ID)).ToList();

                if (tempData.Any())
                {
                    foreach (Order it in tempData)
                    {
                        int index = OrderList.IndexOf(it);
                        OrderList[index].STATUS= Status.getStatusByOrder(it.ID);
                        OrderList[index].TIMELEFT = (DateTime.UtcNow - it.CREATED).ToString(@"dd\.hh\:mm\:ss");
                    }
                }

                foreach (Order it in OrderList)
                {
                    tempIDs.Add(it.ID);
                }

                foreach (OpenOrder it in Bridge.OnHoldOrders)
                {
                    if (tempIDs.Contains(it.OrderUuid))
                    {
                        continue;

                    }
                            System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            OrderList.Add(new Order(it.OrderUuid, it.Exchange, it.QuantityRemaining, it.OrderType, Status.getStatusByOrder(it.OrderUuid), it.Opened, (DateTime.UtcNow - it.Opened).ToString(@"dd\.hh\:mm\:ss")));
                        });
                }

                foreach (OpenOrder it in Bridge.ActiveOrders)
                {
                    if (tempIDs.Contains(it.OrderUuid))
                    {
                        continue;
                    }

                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (!Bridge.OnHoldOrders.Exists(x => x.OrderUuid == it.OrderUuid))
                            OrderList.Add(new Order(it.OrderUuid, it.Exchange, it.QuantityRemaining, it.OrderType, Status.getStatusByOrder(it.OrderUuid), it.Opened, (DateTime.UtcNow - it.Opened).ToString(@"dd\.hh\:mm\:ss")));
                    });
                }

                System.Threading.Thread.Sleep(500);
            }

            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            {
                OrderList.Clear();
            });
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
                    await Task.Delay(10000);
                    continue;
                }

                fFirstDataRetrieval = true;

                foreach (OpenOrder it in Bridge.ActiveOrders)
                {
                    bool fNotCancelling = string.IsNullOrEmpty(Bridge.cancelling.Find(x => x == it.OrderUuid));

                    TimeSpan time = it.Opened.AddDays(Constants.MaxDays) - DateTime.UtcNow;
                    
                    if (time < new TimeSpan(Constants.MaxDays - (int)Properties.Settings.Default["age"], 0, 0, 0) && fNotCancelling)
                    {
                        Task TempTask = new Task(() => remakeOrder(it));
                        TaskingList.Add(TempTask);
                        TempTask.Start();
                    }
                }

                Status.STATUS = "Data retrieved";
                
                try
                {
                    await Task.Delay((int)Properties.Settings.Default["frecuency"] * 3600 * 1000, cancelAwait.Token);
                }
                catch (TaskCanceledException)
                {
                    await Task.Delay(10000);
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

            if (!Bridge.createOrder(order,cancelAwait).Result)
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