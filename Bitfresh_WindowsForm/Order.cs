using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bitfresh_WindowsForm
{
    public class Order : INotifyPropertyChanged
    {
        private string id;
        private string exchange;
        private decimal ammount;
        private string type, status;
        private DateTime created;
        private string timeleft;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string ID { get { return id; } set { if (value != this.id) { this.id = value; NotifyPropertyChanged(); } } }
        public string EXCHANGE { get { return this.exchange; } set { if (value != this.exchange) { this.exchange = value; NotifyPropertyChanged(); } } }
        public decimal AMMOUNT { get { return this.ammount; } set { if (value != this.ammount) { this.ammount = value; NotifyPropertyChanged(); } } }
        public string TYPE { get { return this.type; } set { if (value != this.type) { this.type = value; NotifyPropertyChanged(); } } }
        public string STATUS { get { return this.status; } set { if (value != this.status) { this.status = value; NotifyPropertyChanged(); } } }
        public DateTime CREATED { get { return this.created; } set { if (value != this.created) { this.created = value; NotifyPropertyChanged(); } } }
        public string TIMELEFT { get { return this.timeleft.ToString(); } set { if (value != this.timeleft) { this.timeleft = value; NotifyPropertyChanged(); } } }

        public Order(string _id, string _exchange, decimal _ammount, string _type, string _status, DateTime _created, string _timeleft)
        {
            this.ID = _id;
            this.exchange = _exchange;
            this.AMMOUNT = _ammount;
            this.TYPE = _type;
            this.STATUS = _status;
            this.CREATED = _created;
            this.TIMELEFT = _timeleft;
        }
    }

}
