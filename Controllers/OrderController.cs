using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Repository;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HUSTLEPLUS.SELLER.ORDER.MICROSERVICE.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private IOrders _order;
        public OrderController(IOrders order )
        {
            _order = order;
        }

        [HttpPost("uploadCustomerOrder")]
        public async Task IniateOrderAsync()
        {
            try
            {
            String key = "sk_test_461681dcde84065c1bdb0954fafc48ab5955a524";
                string jsonString;
           
                using (StreamReader reader = new StreamReader(Request.HttpContext.Request.Body, Encoding.UTF8))
                {
                    jsonString = await reader.ReadToEndAsync();
                }
                String inputString = Convert.ToString(new JValue(jsonString));

                String result = "";
                byte[] secretkeyBytes = Encoding.UTF8.GetBytes(key);
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
                using (var hmac = new HMACSHA512(secretkeyBytes))
                {
                    byte[] hashValue = hmac.ComputeHash(inputBytes);
                    result = BitConverter.ToString(hashValue).Replace("-", string.Empty); ;
                }
                Console.WriteLine(result);

                string xpaystackSignature = Request.Headers["x-paystack-signature"];
                if (result.ToLower().Equals(xpaystackSignature))
                {
                    Ok();

                    var request =    JsonConvert.DeserializeObject<PayStackRequest>(inputString);

                  //  string value = request.@event;

                 //   await _order.iniateOrderAsync(request);
                }
                else
                {
                //
                }
            }
            catch (Exception ex)
            {
                string response = ex.Message;
            }

        }

        [HttpPost("test")]
        public async Task test([FromBody]Request request)
        {

            await _order.IniateOrderAsync(request);
        }
    }
}


