using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities
{
    public class History
    {
        public string type { get; set; }
        public string message { get; set; }
        public int time { get; set; }
    }

    public class MetaData
    {
        //public Request Order_Product { get; set; }

    }

    public class CustomerOrders
    {
        public string itemId { get; set; }
        public int    orderQuantity { get; set; }
        public string sellerId { get; set; }
    }
    public class Log
    {
        public int time_spent { get; set; }
        public int attempts { get; set; }
        public string authentication { get; set; }
        public int errors { get; set; }
        public bool success { get; set; }
        public bool mobile { get; set; }
        public List<object> input { get; set; }
        public object channel { get; set; }
        public IList<History> history { get; set; }
    }

    public class Customer
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string customer_code { get; set; }
        public object phone { get; set; }
        public object metadata { get; set; }
        public string risk_action { get; set; }
    }

    public class Authorization
    {
        public string authorization_code { get; set; }
        public string bin { get; set; }
        public string last4 { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string card_type { get; set; }
        public string bank { get; set; }
        public string country_code { get; set; }
        public string brand { get; set; }
        public string account_name { get; set; }
    }

    public class Plan
    {
    }

    public class Data
    {
        public int id { get; set; }
        public string domain { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public int amount { get; set; }
        public object message { get; set; }
        public string gateway_response { get; set; }
        public DateTime paid_at { get; set; }
        public DateTime created_at { get; set; }
        public string channel { get; set; }
        public string currency { get; set; }
        public string ip_address { get; set; }
        public MetaData metadata { get; set; }
        public Log log { get; set; }
        public object fees { get; set; }
        public Customer customer { get; set; }
        public Authorization authorization { get; set; }
        public Plan plan { get; set; }
    }

    public class PayStackRequest
    {
        public string @event { get; set; }
        public Data data { get; set; }
    }
}
