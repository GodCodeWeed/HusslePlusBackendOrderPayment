using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities
{
    public class Request
    {
        public string Order_ID { get; set; }
        public string Buyer_ID { get; set; }
        public string Order_DateTime { get; set; }
        public float Order_TotalCost { get; set; }
        public string Order_CustomersName { get; set; }
        public string Order_ShippingAddress { get; set; }
        public IEnumerable<OrderProduct> Order_Product { get; set; }
        public int Order_State { get; set; }
    }

 

    public class OrderProduct
    {
        public string Order_Status { get; set; }
        public DateTime Order_Expiration_Date { get; set; }
        public DateTime Order_PendingExpiration_Date { get; set; }
        public string Order_SellerID { get; set; }
        public string Order_ProductID { get; set; }
        public float Order_ProductCost { get; set; }
        public int Order_ProductQuantity { get; set; }
        public string Order_ID { get; set; }
    }

    public class OrderProductFufilled: OrderProduct
    {
        public bool Orders_Seller_Payed { get; set; }
        public bool Orders_Shipped { get; set; }
        public DateTime Order_Delivered_Date { get; set; }
        public bool Order_HasIssue { get; set; }
        public bool Order_Recieved { get; set; }
        public string Order_Message { get; set; }
    }
}
