using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                // Obtener el ID del usuario desde el token
                var clienteId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

                var order = await _orderService.CreateOrderAsync(clienteId, request.BookId, request.Cantidad);

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("clients/{bookId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetClientsByBookId(int bookId)
        {
            try
            {
                var clients = await _orderService.GetClientsByBookIdAsync(bookId);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
