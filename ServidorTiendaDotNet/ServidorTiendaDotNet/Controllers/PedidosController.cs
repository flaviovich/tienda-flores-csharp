using System.Collections;
using Microsoft.AspNetCore.Mvc;
using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Extensions;
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

        //[HttpGet("{id}")]
        //public async Task<ActionResult<PedidoResponse>> GetById(int id)
        //{
        //    var pedido=await _pedidoService.GetByIdAsync(id);

        //    if (pedido == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(pedido);
        //}

        [HttpPost]
        public async Task<IActionResult> Create(PedidoCreateDto pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // aprovecha las validaciones automáticas
            }

            var carrito = HttpContext.Session.GetObjectFromJson<List<CarritoItemDto>>("Carrito");
            
            if (carrito?.Any() != true)
            {
                return BadRequest(new { mensaje = "El carrito está vacío o no existe" });
            }
            
            try
            {
                var pedidoCreado = await _pedidoService.PedidoCreateAsync(pedido, carrito);

                HttpContext.Session.Remove("Carrito");

                _logger.LogInformation("Pedido creado exitosamente → ID: {Id}", pedidoCreado.Id);

                return Ok(new 
                { 
                    mensaje = "Pedido registrado correctamente",
                    pedidoId = pedidoCreado.Id,
                    total = pedidoCreado.Total 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear pedido para cliente {Cliente}", pedido.Cliente);
                return StatusCode(500, new { mensaje = "Error interno al procesar el pedido" });
            }
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<PedidoItemResponse>> GetPedidoItemById(int id)
        {
            var detalle = await _pedidoService.GetPedidoItemByIdAsync(id);
            
            return Ok(detalle);
        }
    }
}