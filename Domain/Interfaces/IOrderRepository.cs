using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);
        Task<Book> GetBookByIdAsync(int bookId);
        Task SaveChangesAsync();
    }
}
