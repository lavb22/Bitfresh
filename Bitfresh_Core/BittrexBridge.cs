using BittrexSharp;
using BittrexSharp.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bitfresh_Core
{
    public class BittrexBridge
    {
        private Bittrex account;
        public List<BittrexSharp.Domain.OpenOrder> ActiveOrders;
        public List<BittrexSharp.Domain.OpenOrder> OnHoldOrders;
        public List<string> cancelling;
        public string apiKeyStore;
        public bool fConnected, fFailedConection;

        public BittrexBridge(string apiKey, string apiSecret)
        {
            apiKeyStore = apiKey;
            ActiveOrders = new List<BittrexSharp.Domain.OpenOrder>();
            OnHoldOrders = new List<BittrexSharp.Domain.OpenOrder>();
            cancelling = new List<string>();
            fFailedConection = fConnected = false;

            account = new Bittrex(apiKey, apiSecret); //TODO: Modify library so it uses securestring when handling apiSecret
            //TODO: Maybe change this task so it can use the "testConnection" method
            Task.Run(async () =>
            {
                var response = await account.GetBalances();

                if (!fFailedConection)
                {
                    fFailedConection = !response.Success;
                }

                if (fFailedConection)
                    return;

                fConnected = true;
            });
        }

        public static async Task<bool> testConnection(string apiKey, string apiSecret)
        {
            Bittrex TempAccount = new Bittrex(apiKey, apiSecret);
            var response = await TempAccount.GetBalances();
            TempAccount = null;
            return response.Success;
        }

        public async Task<bool> getCurrentsOrders()
        {
            var response = await account.GetOpenOrders();

            if (!response.Success)
            {
                return false;
            }

            var TempOrders = response.Result as List<BittrexSharp.Domain.OpenOrder>;

            if (TempOrders == null) { ActiveOrders.Clear(); } else { ActiveOrders = TempOrders; }

            return true;
        }

        public async Task<bool> cancelOrder(string uid)
        {
            var aux = ActiveOrders.Find(x => x.OrderUuid == uid);

            if (string.IsNullOrEmpty(aux.OrderUuid))
                return false;

            cancelling.Add(uid);

            var response = await account.CancelOrder(uid);

            if (!response.Success)
            {
                cancelling.Remove(uid);
                return false;
            }

            OnHoldOrders.Add(aux);

            await Task.Run(async () =>
            {
                ResponseWrapper<BittrexSharp.Domain.Order> Response2;
                do
                {
                    await Task.Delay(2 * Constants.second);
                    Response2 = await account.GetOrder(uid);
                } while ((!Response2.Success) || Response2.Result.IsOpen);

                cancelling.Remove(uid);
            });

            return true;
        }

        public async Task<bool> createOrder(OpenOrder orderData, System.Threading.CancellationTokenSource cancelOrderAwait)
        {
            ResponseWrapper<AcceptedOrder> response;

            do
            {
                switch (orderData.OrderType)
                {
                    case "LIMIT_BUY":
                        response = await account.BuyLimit(orderData.Exchange, orderData.QuantityRemaining, orderData.Limit);
                        break;

                    case "LIMIT_SELL":
                        response = await account.SellLimit(orderData.Exchange, orderData.QuantityRemaining, orderData.Limit);
                        break;

                    default:
                        return false;
                }
                await Task.Delay(Constants.second);
            }
            while (!response.Success);

            if (cancelOrderAwait != null) cancelOrderAwait.Cancel();

            while (!ActiveOrders.Exists(x => x.OrderUuid == response.Result.Uuid))
            {
                await Task.Delay(5 * Constants.second);
                if (cancelOrderAwait != null) cancelOrderAwait.Cancel();
            }

            OnHoldOrders.Remove(orderData);

            return true;
        }
    }
}