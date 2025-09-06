using Application.Dtos;
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

        public async Task<OrderResponseDto> CreateOrderAsync(int clienteId, int bookId, int cantidad)
        {
            if (cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a 0.");

            var book = await _orderRepository.GetBookByIdAsync(bookId);
            var cliente = await _orderRepository.GetClienteByIdAsync(clienteId);

            if (book == null)
                throw new Exception("El libro no existe.");
            if (cliente == null)
                throw new Exception("El cliente no existe.");

            if (book.Stock < cantidad)
                throw new Exception("No hay suficiente stock para este libro.");

            // Descontar stock
            book.Stock -= cantidad;
            await _orderRepository.UpdateBookAsync(book);

            var order = new Order
            {
                ClienteId = clienteId,
                BookId = bookId,
                Cantidad = cantidad,
                Fecha = DateTime.UtcNow
            };

            // Guardar orden
            var savedOrder = await _orderRepository.CreateAsync(order);

            // Devolver DTO con la info que se pidio
            return new OrderResponseDto
            {
                ClienteNombre = cliente.Name,
                LibroTitulo = book.Title,
                CategoriaNombre = book.Category?.Name,
                AutorNombre = book.Author?.Name,
                AutorApellido = book.Author?.LastName,
                Cantidad = savedOrder.Cantidad,
                Fecha = savedOrder.Fecha,
                PrecioTotal = savedOrder.Cantidad * book.Price
            };
        }

        // GET con lista de clientes
        public async Task<List<OrderInfoDto>> GetClientsByBookIdAsync(int bookId)
        {
            var orders = await _orderRepository.GetOrdersByBookIdAsync(bookId);

            return orders.Select(o => new OrderInfoDto
            {
                ClienteId = o.ClienteId,
                ClienteNombre = o.Cliente?.Name,
                Cantidad = o.Cantidad,
                PrecioTotal = o.Cantidad * (o.Book?.Price ?? 0),
                Fecha = o.Fecha
            }).ToList();
        }
    }
}
