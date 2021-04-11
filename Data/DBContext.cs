using System;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Data
{
    public class OrderDBContext : DbContext
    {
       
        public OrderDBContext()
        {
        }
        public OrderDBContext(DbContextOptions<OrderDBContext> options)
          : base(options)
        {

        }

        public virtual DbSet<Request> Order { get; set; }
    }
}
