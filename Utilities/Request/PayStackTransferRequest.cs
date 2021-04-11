using System;
using System.Collections.Generic;

namespace HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities
{
    public class PaystackTransfer<T>
    {
        public PaystackTransfer(IEnumerable<T> data)
        {
            transfers = data;
        }

        public IEnumerable<T> transfers { get; set; }
        public string source { get; set; } = "balance";
        public string currency { get; set; } = "NGN";
        public object metadata { get; set; }
    }
}
