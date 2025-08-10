using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<IActionResult> CreateOrder(int bookId, int cantidad)
        {
            try
            {
                // Obtener el ID del usuario desde el token
                var clienteId = int.Parse(User.Claims.First(c => c.Type == "Id").Value);

                var order = await _orderService.CreateOrderAsync(clienteId, bookId, cantidad);

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
