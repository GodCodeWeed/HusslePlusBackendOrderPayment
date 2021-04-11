using System;
using System.Collections.Generic;

namespace HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities
{
    public class BulkTransferPayment
    {
        public bool status { get; set; }
        public string message { get; set; }
        public IEnumerable<object> data { get;set;}
    }
}
