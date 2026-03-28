using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransportOrders.Domain.Model;

namespace TransportOrders.Infrasrtructure.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
       : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
