using Microsoft.AspNetCore.Mvc;
using ServidorTiendaDotNet.Models;
using ServidorTiendaDotNet.Extensions;
using ServidorTiendaDotNet.Services;

namespace ServidorTiendaDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
        private readonly ILogger<CarritoController> _logger;
        private readonly IFlorService _florService;

        public CarritoController(ILogger<CarritoController> logger,
            IFlorService florService)
        {
            _logger = logger;
            _florService = florService;
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> AgregarAlCarrito(CarritoDTO dto)
        {
            _logger.LogInformation("Petición para agregar al carrito recibida");

            if (dto == null)
            {
                return BadRequest("No se recibió ningún dato en el body");
            }

            // Extraer datos del DTO
            int florId = dto.FlorId;
            int cantidad = 1;

            if (dto is not null && dto.Cantidad is int c && c > 0)
            {
                cantidad = c;
            }

            if (florId <= 0)
            {
                return BadRequest("El identificador de la flor (florId) es obligatorio y debe ser mayor que 0");
            }

            if (cantidad < 1)
            {
                return BadRequest("La cantidad debe ser mayor o igual a 1");
            }

            List<Carrito>? carrito = HttpContext.Session.GetObjectFromJson<List<Carrito>>("Carrito");
            var carritoExistente = carrito?.FirstOrDefault();

            if (carritoExistente == null)
            {
                carritoExistente = new Carrito { Id = 1 };
                carrito = new List<Carrito> { carritoExistente };
            }

            // Buscar en la bd
            var flor = await _florService.GetByIdAsync(florId);

            if (flor == null)
            {
                return NotFound($"No se encontró la flor con id {florId}");
            }

            var item = carritoExistente.Items.FirstOrDefault(i => i.FlorId == flor.Id);

            if (item == null)
            {
                item = new CarritoItem
                {
                    FlorId = flor.Id,
                    Flor = flor,
                    Cantidad = cantidad,
                    PrecioUnitario = flor.Precio
                };
                carritoExistente.Items.Add(item);
            }
            else
            {
                item.Cantidad += cantidad;
            }

            HttpContext.Session.SetObjectAsJson("Carrito", carrito);

            return Ok(new
            {
                mensaje = $"Agregado {cantidad} unidad(es) de la flor {florId}",
                totalItems = carritoExistente.CantidadItems,
                total = carritoExistente.Total,
                carrito = carritoExistente
            });
        }

        [HttpGet]
        public IActionResult ObtenerCarrito()
        {
            _logger.LogInformation("Se ha recibido una petición para obtener el carrito.");

            List<Carrito>? carrito = HttpContext.Session.GetObjectFromJson<List<Carrito>>("Carrito");
            var carritoExistente = carrito?.FirstOrDefault();
            
            if (carritoExistente == null)
            {
                carritoExistente = new Carrito { Id = 1 };
            }

            return Ok(carritoExistente);
        }

        [HttpPost("vaciar")]
        public IActionResult VaciarCarrito()
        {
            _logger.LogInformation("Petición para vaciar el carrito recibida");

            HttpContext.Session.Remove("Carrito");

            return Ok(new
            {
                mensaje = "Carrito vaciado correctamente"
            });
        }

    }
}
