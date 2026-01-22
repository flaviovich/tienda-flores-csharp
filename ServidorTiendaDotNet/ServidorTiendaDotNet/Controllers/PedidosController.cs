using Microsoft.AspNetCore.Mvc;
using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Extensions;
using ServidorTiendaDotNet.Models;
using ServidorTiendaDotNet.Services;

namespace ServidorTiendaDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        readonly ILogger<PedidosController> _logger;
        readonly IPedidoService _pedidoService;

        public PedidosController(ILogger<PedidosController> logger, IPedidoService pedido)
        {
            _logger = logger;
            _pedidoService = pedido;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PedidoResponse>>> GetAll()
        {
            _logger.LogInformation("Se ha recibido una petición a /api/pedidos");
            var flores = await _pedidoService.GetAllAsync();

            return Ok(flores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoResponse>> GetById(int id)
        {
            var pedido=await _pedidoService.GetByIdAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePedido(PedidoCreateDto pedido)
        {
            _logger.LogInformation("Petición para registrar pedido recibida");

            List<CarritoResponse>? carrito = HttpContext.Session.GetObjectFromJson<List<CarritoResponse>>("Carrito");

            if (carrito == null || !carrito.Any())
            {
                return BadRequest("El carrito está vacío. No se puede registrar el pedido.");
            }

            List<PedidoCreateDto>? pedidos = HttpContext.Session.GetObjectFromJson<List<PedidoCreateDto>>("Pedidos");

            if (pedidos == null)
            {
                pedidos = new List<PedidoCreateDto>();
            }
            pedidos.Add(pedido);

            HttpContext.Session.SetObjectAsJson("Pedidos", pedidos);
            HttpContext.Session.Remove("Carrito");

            var pedidoNew = await _pedidoService.CreateAsync(pedido, carrito.First());

            return Ok(new { mensaje = "Pedido registrado exitosamente", id = pedidoNew.Id });
        }

        //[HttpGet("{id}")]
        //public PedidoDetalle ObtenerDetallesPedido(int pedidoId)
        //{
        //    PedidoDetalle detallePedido = new PedidoDetalle();
        //    detallePedido.pedido = _pedidoService.GetById(pedidoId).Result;
        //}
    }
}