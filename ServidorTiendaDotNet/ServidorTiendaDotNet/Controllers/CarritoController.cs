using Microsoft.AspNetCore.Mvc;
using ServidorTiendaDotNet.DTOs;
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

        public CarritoController(ILogger<CarritoController> logger, IFlorService florService)
        {
            _logger = logger;
            _florService = florService;
        }

        [HttpGet]
        public async Task<ActionResult<CarritoResponse>> GetCarrito()
        {
            _logger.LogInformation("Petición para obtener carrito");

            // Leer el mismo tipo que se guarda en POST
            var items = HttpContext.Session.GetObjectFromJson<List<CarritoItemDto>>("Carrito")
                        ?? new List<CarritoItemDto>();

            var response = new CarritoResponse
            {
                Items = new List<CarritoItemResponse>(),
                TotalItems = 0,
                Total = 0m
            };

            foreach (var itemDto in items)
            {
                var flor = await _florService.GetByIdAsync(itemDto.FlorId);
                if (flor == null) continue;

                var itemResponse = new CarritoItemResponse
                {
                    FlorId = itemDto.FlorId,
                    Nombre = flor.Nombre,
                    PrecioUnitario = flor.Precio,
                    Cantidad = itemDto.Cantidad,
                };

                response.Items.Add(itemResponse);
                response.TotalItems += itemDto.Cantidad;
                response.Total += itemDto.Cantidad * flor.Precio;
            }

            return Ok(response);
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
        public async Task<ActionResult<CarritoResponse>> CreateCarritoItem([FromBody] CarritoItemDto dto)
        {
            if (dto == null || dto.FlorId <= 0 || dto.Cantidad < 1)
            {
                return BadRequest("Datos inválidos");
            }

            // Recuperar o inicializar la lista
            var carritoList = HttpContext.Session.GetObjectFromJson<List<CarritoItemDto>>("Carrito")
                              ?? new List<CarritoItemDto>();

            // Buscar si ya existe el producto en el carrito
            var itemExistente = carritoList.FirstOrDefault(x => x.FlorId == dto.FlorId);

            if (itemExistente != null)
            {
                // ya existe → incrementar cantidad
                itemExistente.Cantidad += dto.Cantidad;
            }
            else
            {
                // no existe → añadir nuevo ítem
                var flor = await _florService.GetByIdAsync(dto.FlorId);
                if (flor == null) return NotFound();

                carritoList.Add(new CarritoItemDto
                {
                    FlorId = dto.FlorId,
                    Cantidad = dto.Cantidad,
                });
            }

            // Guardar de nuevo en sesión
            HttpContext.Session.SetObjectAsJson("Carrito", carritoList);

            var response = new CarritoResponse
            {
                Items = new List<CarritoItemResponse>(),
                TotalItems = 0,
                Total = 0m,
                Mensaje = "Producto añadido correctamente"
            };

            foreach (var itemDto in carritoList)
            {
                var flor = await _florService.GetByIdAsync(itemDto.FlorId);

                if (flor == null) continue;

                var itemResponse = new CarritoItemResponse
                {
                    FlorId = itemDto.FlorId,
                    Nombre = flor.Nombre,
                    PrecioUnitario = flor.Precio,
                    Cantidad = itemDto.Cantidad,
                };

                response.Items.Add(itemResponse);
                response.TotalItems += itemDto.Cantidad;
                response.Total += itemDto.Cantidad * flor.Precio;
            }

            return Ok(response);
        }
    }
}