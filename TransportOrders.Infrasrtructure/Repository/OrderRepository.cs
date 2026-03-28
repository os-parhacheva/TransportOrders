using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportOrders.Domain.Model;
using TransportOrders.Infrasrtructure.Data;

namespace TransportOrders.Infrasrtructure.Repository
{
    public class OrderRepository
    {
        private readonly Context _context;

        public Context UnitOfWork
        {
            get
            {
                return _context;
            }
        }
        public OrderRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();            
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Orders.SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            Order existOrder = await _context.Orders.SingleOrDefaultAsync(i => i.Id == order.Id);
            _context.Entry(existOrder).CurrentValues.SetValues(order);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            Order order = await _context.Orders.FindAsync(id);
            _context.Remove(order);

            await _context.SaveChangesAsync();
        }
    }
}
