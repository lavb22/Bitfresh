using Bitfresh_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bitfresh_WindowsForm
{
    public class StatusClass : IStatus, INotifyPropertyChanged
    {
        private Dictionary<string, string> statusByOrder;
        private string status;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string STATUS
        {
            get { return status; }
            set
            {
                if (value != this.status)
                {
                    this.status = value; Program.MainUI.refcontrol.Invoke((System.Windows.Forms.MethodInvoker)delegate
                    {
                        if (Program.MainUI.fMonoFramework) { Program.MainUI.ChangeStatusLabel(value); } else { NotifyPropertyChanged(); }
                    });
                }
            }
        }

        public StatusClass()
        {
            status = "Waiting...";
            statusByOrder = new Dictionary<string, string>();
        }

        public string getStatusByOrder(string uid)
        {
            string returnedStatus;

            if (statusByOrder.TryGetValue(uid, out returnedStatus))
            {
                return returnedStatus;
            }

            return "Stand By";
        }

        public void setStatusByOrder(string uid, string status)
        {
            try
            {
                statusByOrder[uid] = status;
            }
            catch (KeyNotFoundException)
            {
                statusByOrder.Add(uid, status);
            }
        }

        public void removeStatusByOrder(string uid)
        {
            statusByOrder.Remove(uid);
        }
    }
}