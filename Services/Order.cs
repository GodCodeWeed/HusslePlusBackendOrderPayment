using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Repository;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Services
{
    public class Order: IOrders
    {
        private readonly IConfiguration _configuration;

        public Order(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task IniateOrderAsync(Request model)
        {
            string json = JsonConvert.SerializeObject(model, Formatting.Indented);
     
           // string json = JsonConvert.SerializeObject(model.Order_Product, Formatting.Indented);
            //if (model.@event.Equals("charge.success"))
            //{

            // SqlConnection _connection = new SqlConnection();
            SqlTransaction transaction;
                int response = 0;
                using (SqlConnection _connection = new (_configuration.GetConnectionString("OrderConnectionString")))
                {
                    _connection.Open();
                    try
                    {
                        using (SqlCommand cmd = new ("SP_Customer_NewOrder", _connection))
                        {
                            transaction = _connection.BeginTransaction("OrderCreateTransaction");
                            cmd.Transaction = transaction;
                            cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@Order_ID", model.data.metadata.Order_Product.Order_ID);
                            //cmd.Parameters.AddWithValue("@Buyer_ID", model.data.metadata.Order_Product.Buyer_ID);
                            //cmd.Parameters.AddWithValue("@Order_DateTime", model.data.metadata.Order_Product.Order_DateTime);
                            //cmd.Parameters.AddWithValue("@Order_TotalCost", model.data.metadata.Order_Product.Order_TotalCost);
                            //cmd.Parameters.AddWithValue("@Order_CustomersName", model.data.metadata.Order_Product.Order_CustomersName);
                            //cmd.Parameters.AddWithValue("@Order_ShippingAddress", model.data.metadata.Order_Product.Order_ShippingAddress);
                            //cmd.Parameters.AddWithValue("@Order_State", model.data.metadata.Order_Product.Order_State);
                      
                            cmd.Parameters.AddWithValue("@Order_ID", model.Order_ID);
                            cmd.Parameters.AddWithValue("@Buyer_ID", model.Buyer_ID);
                            cmd.Parameters.AddWithValue("@Order_DateTime", model.Order_DateTime);
                            cmd.Parameters.AddWithValue("@Order_TotalCost", model.Order_TotalCost);
                            cmd.Parameters.AddWithValue("@Order_CustomersName", model.Order_CustomersName);
                            cmd.Parameters.AddWithValue("@Order_ShippingAddress", model.Order_ShippingAddress);
                            cmd.Parameters.AddWithValue("@Order_State", model.Order_State);
                            response = cmd.ExecuteNonQuery();
                        }
                        if (response > 0) {

                            using (SqlCommand cmd2 = new SqlCommand())
                            {
                            cmd2.Connection = _connection;
                            cmd2.Transaction = transaction;
                            cmd2.CommandText = "SP_Customer_NewOrder_Item";
                            cmd2.CommandType = CommandType.StoredProcedure;
                            //     "SP_Customer_NewOrder_Item"
                            try
                                {
                                    foreach (var order in model.Order_Product)
                                    { 
                                        cmd2.Parameters.AddWithValue("@order_Status", order.Order_Status);
                                        cmd2.Parameters.AddWithValue("@Order_SellerID", order.Order_SellerID );
                                        cmd2.Parameters.AddWithValue("@Order_ProductID",order.Order_ProductID );
                                        cmd2.Parameters.AddWithValue("@Order_ProductsCost", order.Order_ProductCost);
                                        cmd2.Parameters.AddWithValue("@Order_ProductQuantity",order.Order_ProductQuantity );
                                        cmd2.Parameters.AddWithValue("@Order_ActiveExpiration_Date", DateTime.Now.AddDays(3));
                                        cmd2.Parameters.AddWithValue("@Order_PendingExpiration_Date", DateTime.Now.AddDays(1));
                                        cmd2.Parameters.AddWithValue("@Order_ID",order.Order_ID);
                                        if (_connection.State == ConnectionState.Open)
                                            cmd2.ExecuteNonQuery();

                                        cmd2.Parameters.Clear();

                                    }

                                    transaction.Commit();
                                }
                                catch (SqlException ex)
                                {
                               
                                    transaction.Rollback();
                                }
                            }
           

                        }
                  
                        //return ("Data save Successfully");
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        if (_connection.State == ConnectionState.Open)
                            _connection.Close();
                    }

                }
          // }

      }

        public async Task ReconciliationAsync()
        {

            double sellerPercent = 90;
            double husslePercent = 10;
            double totalPercent = 100;
            double sumPayOut = 0;
            double amount = 0;
            List<PaymentFulliment> resultList = new List<PaymentFulliment>();
            List<string> orderIds = new List<string>();
            using (SqlConnection connection = new())
            {

                connection.ConnectionString = _configuration.GetConnectionString("OrderConnectionString");
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (SqlCommand command = new())
                    {
                        command.Connection = connection;
                        command.CommandText = "PayFufilledOrder";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Order_SellerID", "mike1");
                        //  command.CommandType = CommandType.StoredProcedure;


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                PaymentFulliment paymentFulliment = new();
                           
                                amount = double.Parse(reader["Order_ProductsCost"].ToString());
                                paymentFulliment.amount = (int) Math.Floor((sellerPercent / totalPercent) * amount);
                                sumPayOut += (husslePercent / totalPercent) * amount;
                                paymentFulliment.recipient = (reader["SellerAccount"]) as string;
                                paymentFulliment.reason = "Your Hussle don pay for " + (reader["Order_ID"]) as string;
                                orderIds.Add((reader["Order_ID"]) as string);
                                resultList.Add(paymentFulliment);
                            }

                        }
                    }
                   
                    if (resultList.Count > 0)
                    {
                        //var HusslePayOut = new PaymentFulliment
                        //{
                        //    amount = (int)Math.Floor(sumPayOut),
                        //    reason = "Hussle Weekly Payout",
                        //    recipient = ""

                        //};

                        //resultList.Add(HusslePayOut);

                        PaystackTransfer<PaymentFulliment> transfer = new PaystackTransfer<PaymentFulliment>(resultList);
                               
                    
                        var json = JsonConvert.SerializeObject(transfer);

                      
                        //var metaData =     
                        var data = new StringContent(json, Encoding.UTF8, "application/json");
                        var url = "https://api.paystack.co/transfer/bulk";
                        using HttpClient client = new();
                        client.DefaultRequestHeaders.Add("Authorization", "bearer sk_test_461681dcde84065c1bdb0954fafc48ab5955a524");
                      //  client.DefaultRequestHeaders.Add("metadata", "1");
                        var response = await client.PostAsync(url, data);

                     //  client.DefaultRequestHeaders.Add("metadata", resultList );
                        string result = response.Content.ReadAsStringAsync().Result;

                      var resultData =   JsonConvert.DeserializeObject<BulkTransferPayment>(result);

                        if (!resultData.status)
                        {
                        
                        updatePaymentrecords(orderIds);


                       }

                    }
                }
                catch (Exception ex)
                {


                }
            }

        }

        private void updatePaymentrecords(List<string> orderIds)
        {
            SqlTransaction transaction;
            using (SqlConnection _connection = new(_configuration.GetConnectionString("OrderConnectionString")))
            {
                _connection.Open();
                using (SqlCommand cmd2 = new SqlCommand())
                {
                    transaction = _connection.BeginTransaction("PaymentCreateTransaction");
                    cmd2.Connection = _connection;
                    cmd2.Transaction = transaction;
                    cmd2.CommandText = "PayOutsToSellersFailed";
                    cmd2.CommandType = CommandType.StoredProcedure;
                    //     "SP_Customer_NewOrder_Item"
                    try
                    {
                        foreach (var id in orderIds)
                        {
                            cmd2.Parameters.AddWithValue("@Order_ID", id);

                            if (_connection.State == ConnectionState.Open)
                               cmd2.ExecuteNonQuery();

                            cmd2.Parameters.Clear();
                        }
                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {

                        transaction.Rollback();
                    }
                    finally
                    {
                        _connection.Close();


                    }
                }

            }

  
        }
 
        public async Task FufillmentCronJOb()
        {
         

                using (SqlConnection connection = new(_configuration.GetConnectionString("OrderConnectionString")))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        using (SqlCommand command = new())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Connection = connection;
                            command.CommandText = "SP_Order_Expiration";
                            if (connection.State == ConnectionState.Open)
                                await command.ExecuteNonQueryAsync();

                        }

                    }
                    catch (SqlException ex)
                    {
                        
                    }
                    finally
                    {

                        connection.Close();

                    }
                }
 


        }
    
    }
}
