using BittrexSharp.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Bittrex_refresh
{
    public class BackupUtility : IDisposable
    {
        private string CurrentPath, Bktarget, apiKeyStore;
        private FileStream filestream;
        private List<SerialOpenOrder> BackupOpenOrders, MyBackupOpenOrders;
        private SerialOpenOrder norder;

        private IFormatter formatter;

        public BackupUtility(string apikey)
        {
            apiKeyStore = apikey;
            CurrentPath = Directory.GetCurrentDirectory();
            Bktarget = CurrentPath + "\\Backup.dat";

            if (!File.Exists(Bktarget))
            {
                File.Create(Bktarget).Close();
            }

            BackupOpenOrders = new List<SerialOpenOrder>();
            MyBackupOpenOrders = new List<SerialOpenOrder>();

            filestream = new FileStream(Bktarget, FileMode.Open);

            formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            norder = new SerialOpenOrder();

            this.getOrders();
        }

        public bool ordersToRestore()
        {
            return MyBackupOpenOrders.Any();
        }

        public List<OpenOrder> Get()
        {
            this.getOrders();
            List<OpenOrder> auxList = new List<OpenOrder>();

            foreach (SerialOpenOrder it in MyBackupOpenOrders)
            {
                auxList.Add(it.Get());
            }

            return auxList;
        }

        public void writeOrder(OpenOrder order)
        {
            filestream.Position = filestream.Length;

            norder.Assign(apiKeyStore, order);

            formatter.Serialize(filestream, norder);

            return;
        }

        public void writeOrder(SerialOpenOrder order)
        {
            formatter.Serialize(filestream, order);

            return;
        }

        public void getOrders()
        {
            BackupOpenOrders.Clear();
            MyBackupOpenOrders.Clear();

            filestream.Position = 0;
            while (filestream.Position != filestream.Length)
            {
                SerialOpenOrder r = formatter.Deserialize(filestream) as SerialOpenOrder;

                BackupOpenOrders.Add(r);

                if (r.ApiKey == this.apiKeyStore)
                {
                    MyBackupOpenOrders.Add(r);
                }
            }
            return;
        }

        public void eraseOrder(List<string> order)
        {
            this.getOrders();

            filestream.Close();
            filestream = new FileStream(Bktarget, FileMode.Create);

            foreach (SerialOpenOrder it in BackupOpenOrders)
            {
                if (!(order.Contains(it.OrderUuid)))
                    this.writeOrder(it);
            }

            order.Clear();
            return;
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    filestream.Close();
                    filestream.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}

[Serializable]
public class SerialOpenOrder
{
    public string ApiKey { get; set; }
    public bool IsConditional { get; set; }
    public bool ImmediateOrCancel { get; set; }
    public bool CancelInitiated { get; set; }
    public DateTime? Closed { get; set; }
    public DateTime Opened { get; set; }
    public decimal? PricePerUnit { get; set; }
    public decimal Price { get; set; }
    public decimal CommissionPaid { get; set; }
    public decimal Limit { get; set; }
    public decimal QuantityRemaining { get; set; }
    public decimal Quantity { get; set; }
    public string OrderType { get; set; }
    public string Exchange { get; set; }
    public string OrderUuid { get; set; }
    public string Uuid { get; set; }
    public string Condition { get; set; }
    public string ConditionTarget { get; set; }

    public SerialOpenOrder()
    {
        this.ApiKey = "";
    }

    public void Assign(string api, OpenOrder order)
    {
        this.ApiKey = api;
        this.CancelInitiated = order.CancelInitiated;
        this.Closed = order.Closed;
        this.CommissionPaid = order.CommissionPaid;
        this.Condition = order.Condition;
        this.ConditionTarget = order.Exchange;
        this.Exchange = order.Exchange;
        this.Limit = order.Limit;
        this.Opened = order.Opened;
        this.OrderType = order.OrderType;
        this.OrderUuid = order.OrderUuid;
        this.Price = order.Price;
        this.QuantityRemaining = order.QuantityRemaining;
        this.Uuid = order.Uuid;
        this.Quantity = order.Quantity;
        this.PricePerUnit = order.PricePerUnit;
        this.IsConditional = order.IsConditional;
        this.ImmediateOrCancel = order.ImmediateOrCancel;
    }

    public OpenOrder Get()

    {
        OpenOrder auxOrder = new OpenOrder
        {
            CancelInitiated = this.CancelInitiated,
            Closed = this.Closed,
            CommissionPaid = this.CommissionPaid,
            Condition = this.Condition,
            ConditionTarget = this.Exchange,
            Exchange = this.Exchange,
            Limit = this.Limit,
            Opened = this.Opened,
            OrderType = this.OrderType,
            OrderUuid = this.OrderUuid,
            Price = this.Price,
            QuantityRemaining = this.QuantityRemaining,
            Uuid = this.Uuid,
            Quantity = this.Quantity,
            PricePerUnit = this.PricePerUnit,
            IsConditional = this.IsConditional,
            ImmediateOrCancel = this.ImmediateOrCancel
        };

        return auxOrder;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
}