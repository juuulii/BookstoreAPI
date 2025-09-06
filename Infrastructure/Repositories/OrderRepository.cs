using Domain.Entities;
using Domain.Interfaces;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationContext _context;

        public OrderRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _context.Books
                .Include(b => b.Author)      
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task<User> GetClienteByIdAsync(int clienteId) 
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == clienteId && !u.IsDeleted);
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersByBookIdAsync(int bookId)
        {
            return await _context.Orders
                .Include(o => o.Cliente)
                .Where(o => o.BookId == bookId)
                .Include(o => o.Book)
                .ToListAsync();
        }
    }
}
