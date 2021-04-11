using System;
using System.Threading.Tasks;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities;

namespace HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Repository
{
    public interface IOrders
    {
        Task IniateOrderAsync(Request model);
        Task FufillmentCronJOb();
        Task ReconciliationAsync();
        //
    }
}
