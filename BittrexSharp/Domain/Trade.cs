﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BittrexSharp.Domain
{
    public class Trade
    {
        public string MarketName { get; set; }
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string FillType { get; set; }
        public string OrderType { get; set; }
    }
}
