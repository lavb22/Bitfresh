using Bitfresh_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Bitfresh_WindowsForm
{
    public partial class MainWindow
    {
        private void closingTask()
        {
            refcontrol.BeginInvoke((MethodInvoker)delegate ()
            {
                ConnectButton.Enabled = false;
            });
            if (Manager != null)
            {
                if (Manager.CheckifTask())
                {
                    if (!fMonoFramework)
                    {
                        AppNotifyIcon.BalloonTipText = "Bitfresh is closing. Please wait until running tasks complete.";
                        AppNotifyIcon.ShowBalloonTip(3 * Constants.second);
                    }
                    refcontrol.BeginInvoke((MethodInvoker)delegate ()
                    {
                        IsEnabled = false;
                    });
                }

                Manager.Dispose();
            }

            if (!fMonoFramework)
            {
                NotifyMenu.Dispose();
                AppNotifyIcon.Dispose();
            }

            refcontrol.BeginInvoke((MethodInvoker)delegate ()
            {
                PrfWindow.Dispose();
                Application.Exit();
            });
        }

        private void disconnectingTask(StatusClass _ErrorStatus)
        {
            refcontrol.BeginInvoke((MethodInvoker)delegate ()
            {
                this.ConnectButton.Text = "Disconnecting";
            });

            Manager.Dispose();

            refcontrol.BeginInvoke((MethodInvoker)delegate ()
            {
                ConnectButton.Enabled = true;
                this.ConnectButton.Text = "Connect";
                ProfManagerButton.Enabled = true;
                UsersComboBox.Enabled = true;
                _ErrorStatus.STATUS = "Waiting...";
                this.confBtn.Enabled = true;
            });
        }

        public void ShowConnectionError()
        {
            MessageBox.Show(this, "Failed to connecto to servers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Manager.Dispose();
            ErrorStatus.STATUS = "Waiting...";
            this.ConnectButton.Enabled = true;
            this.ConnectButton.Text = "Connect";
            ProfManagerButton.Enabled = true;
            UsersComboBox.Enabled = true;
        }

        public void AskForBackup()
        {
            DialogResult re = MessageBox.Show(this, "Backup detected: Do you want to restore it?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (re == DialogResult.Yes)
            {
                Manager.fRestoreBackup = true;
            }
            else
            {
                Manager.fDeleteBackup = true;
            }

            Manager.startTasks();
        }

        private void UpdateGUI(StatusClass _ErrorStatus)
        {
            List<string> tempIDs = new List<string>();
            List<Order> tempData = new List<Order>();

            bool fonlyOnce = true;
            while (!Manager.fStop)
            {
                if (Manager.Bridge.fFailedConection)
                {
                    Manager.fStop = true;

                    refcontrol.BeginInvoke((MethodInvoker)delegate ()
                    {
                        ShowConnectionError();
                    });
                }

                if (!Manager.Bridge.fConnected)
                {
                    System.Threading.Thread.Sleep(500);
                    continue;
                }

                if (fonlyOnce)
                {
                    if (Manager.bkp.ordersToRestore())
                    {
                        refcontrol.BeginInvoke((MethodInvoker)delegate ()
                        {
                            AskForBackup();
                        });
                    }
                    else
                    {
                        Manager.startTasks();
                    }
                    fonlyOnce = false;
                }

                tempData = OrderList.Where(el => !(Manager.Bridge.OnHoldOrders.Exists(x => x.OrderUuid == el.ID) || Manager.Bridge.ActiveOrders.Exists(x => x.OrderUuid == el.ID))).ToList();

                if (tempData.Any())
                {
                    foreach (Order it in tempData)
                    {
                        refcontrol.BeginInvoke((MethodInvoker)delegate ()
                        {
                            OrderList.Remove(it);
                        });
                    }
                }

                tempData = OrderList.Where(el => Manager.Bridge.OnHoldOrders.Exists(x => x.OrderUuid == el.ID) || Manager.Bridge.ActiveOrders.Exists(x => x.OrderUuid == el.ID)).ToList();

                if (tempData.Any())
                {
                    foreach (Order it in tempData)
                    {
                        int index = OrderList.IndexOf(it);
                        OrderList[index].STATUS = ErrorStatus.getStatusByOrder(it.ID);
                        OrderList[index].TIMELEFT = (DateTime.UtcNow - it.CREATED).ToString(@"dd\.hh\:mm\:ss");
                    }
                }

                foreach (Order it in OrderList)
                {
                    tempIDs.Add(it.ID);
                }

                foreach (BittrexSharp.Domain.OpenOrder it in Manager.Bridge.OnHoldOrders)
                {
                    if (tempIDs.Contains(it.OrderUuid))
                    {
                        continue;
                    }
                    refcontrol.BeginInvoke((MethodInvoker)delegate ()
                    {
                        OrderList.Add(new Order(it.OrderUuid, it.Exchange, it.QuantityRemaining, it.OrderType, ErrorStatus.getStatusByOrder(it.OrderUuid), it.Opened, (DateTime.UtcNow - it.Opened).ToString(@"dd\.hh\:mm\:ss")));
                    });
                }

                foreach (BittrexSharp.Domain.OpenOrder it in Manager.Bridge.ActiveOrders)
                {
                    if (tempIDs.Contains(it.OrderUuid))
                    {
                        continue;
                    }

                    refcontrol.BeginInvoke((MethodInvoker)delegate ()
                    {
                        if (!Manager.Bridge.OnHoldOrders.Exists(x => x.OrderUuid == it.OrderUuid))
                            OrderList.Add(new Order(it.OrderUuid, it.Exchange, it.QuantityRemaining, it.OrderType, ErrorStatus.getStatusByOrder(it.OrderUuid), it.Opened, (DateTime.UtcNow - it.Opened).ToString(@"dd\.hh\:mm\:ss")));
                    });
                }

                tempIDs.Clear();
                System.Threading.Thread.Sleep(Constants.second);
            }

            refcontrol.BeginInvoke((MethodInvoker)delegate ()
            {
                OrderList.Clear();
            });
        }

        public void ChangeStatusLabel(string text)
        {
            StatusLabel.Text = text;
        }
    }
}