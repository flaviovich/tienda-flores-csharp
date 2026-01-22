using Microsoft.AspNetCore.Mvc;
using ServidorTiendaDotNet.Extensions;
using ServidorTiendaDotNet.Models;
using ServidorTiendaDotNet.Services;

namespace ServidorTiendaDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Tags("carrito")]
    public class CarritoController : ControllerBase
    {
        readonly ILogger<CarritoController> _logger;
        readonly IFlorService _florService;

        public CarritoController(ILogger<CarritoController> logger,
            IFlorService florService)
        {
            _logger = logger;
            _florService = florService;
        }

        [HttpGet]
        public ActionResult<CarritoResponse> GetCarrito()
        {
            _logger.LogInformation("Se ha recibido una petición para obtener el carrito.");

            var carrito = HttpContext.Session.GetObjectFromJson<List<Carrito>>("Carrito")?
                .FirstOrDefault() ?? new Carrito { Id = 1 };

            return Ok(carrito.ToResponse());
        }

        [HttpDelete]
        public IActionResult DeleteCarrito()
        {
            _logger.LogInformation("Petición para vaciar el carrito recibida");

            HttpContext.Session.Remove("Carrito");

            var response = new ApiMessageResponse
            {
                Mensaje = "Carrito vaciado correctamente.",
                TotalItems = 0,
                Total = 0m
            };

            return Ok(response);
        }

        [HttpPost("items")]
        public async Task<ActionResult<CarritoResponse>> AddCarritoItem([FromBody] AddCartItemDto dto)
        {
            _logger.LogInformation("Petición para agregar al carrito recibida");

            if (dto == null)
            {
                return BadRequest("No se recibió ningún dato en el body");
            }

            // Extraer datos del DTO
            int florId = dto.FlorId;

            if (florId <= 0)
            {
                return BadRequest("El identificador de la flor (florId) es obligatorio y debe ser mayor que 0");
            }

            if (dto.Cantidad < 1)
            {
                return BadRequest("La cantidad debe ser mayor o igual a 1");
            }

            // Buscar en la bd
            var flor = await _florService.GetByIdAsync(florId);

            if (flor == null)
            {
                return NotFound($"No se encontró la flor con id {florId}");
            }

            var carritoList = HttpContext.Session.GetObjectFromJson<List<Carrito>>("Carrito");
            var carrito = carritoList?.FirstOrDefault() ?? new Carrito { Id = 1 };

            var item = carrito.Items.FirstOrDefault(i => i.FlorId == flor.Id);

            if (item == null)
            {
                var newItem = new CarritoItem
                {
                    FlorId = flor.Id,
                    Flor = flor,
                    Cantidad = dto.Cantidad,
                    PrecioUnitario = flor.Precio
                };
                carrito.Items.Add(newItem);
            }
            else
            {
                item.Cantidad += dto.Cantidad;
            }

            HttpContext.Session.SetObjectAsJson("Carrito", new List<Carrito> { carrito });

            return Ok(carrito.ToResponse());
        }
    }
}