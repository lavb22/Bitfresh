using Newtonsoft.Json.Linq;

namespace BittrexSharp
{
    internal class BittrexResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public JToken Result { get; set; }
    }
}