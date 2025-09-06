using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);
        Task<Book> GetBookByIdAsync(int bookId);
        Task<User> GetClienteByIdAsync(int clienteId);
        Task UpdateBookAsync(Book book);
        Task<List<Order>> GetOrdersByBookIdAsync(int bookId);

    }
}

