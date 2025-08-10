using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(int clienteId, int bookId, int cantidad)
        {
            var book = await _orderRepository.GetBookByIdAsync(bookId);

            if (book == null)
                throw new Exception("El libro no existe.");

            if (book.Stock < cantidad)
                throw new Exception("No hay suficiente stock para este libro.");

            // Descontar stock
            book.Stock -= cantidad;

            var order = new Order
            {
                ClienteId = clienteId,
                BookId = bookId,
                Cantidad = cantidad,
                Fecha = DateTime.UtcNow
            };

            await _orderRepository.CreateAsync(order);

            return order;
        }
    }
}
