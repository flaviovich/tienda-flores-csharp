using Microsoft.AspNetCore.Mvc;
using ServidorTiendaDotNet.Models;
using ServidorTiendaDotNet.Extensions;

namespace ServidorTiendaDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
        private readonly ILogger<CarritoController> _logger;

        public CarritoController(ILogger<CarritoController> logger)
        {
            _logger = logger;
        }

        [HttpPost("agregar")]
        public IActionResult AgregarAlCarrito([FromBody] Dictionary<string, object> datos)
        {
            _logger.LogInformation("Petición para agregar al carrito recibida");

            if (datos == null)
            {
                return BadRequest("No se recibió ningún dato en el body");
            }

            // Extraer florId
            if (!datos.TryGetValue("florId", out var florIdObj) ||
                florIdObj == null ||
                !int.TryParse(florIdObj.ToString(), out int florId))
            {
                return BadRequest("El campo 'florId' es obligatorio y debe ser un número entero");
            }

            // Extraer cantidad (opcional → por defecto 1)
            int cantidad = 1;
            if (datos.TryGetValue("cantidad", out var cantidadObj) &&
                cantidadObj != null &&
                int.TryParse(cantidadObj.ToString(), out int cantTemp))
            {
                cantidad = cantTemp;
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

            // Buscar la flor en el catálogo disponible (sesión u otra fuente)
            List<Flor>? catalogo = HttpContext.Session.GetObjectFromJson<List<Flor>>("Flores");

            Flor? flor = catalogo?.FirstOrDefault(f => f.Id == florId);

            if (flor == null)
            {
                // Si no hay catálogo en sesión, no se puede buscar en DB aquí. Devolver error claro.
                return NotFound($"No se encontró la flor con id {florId}");
            }

            // Agregar la misma flor "cantidad" veces
            for (int i = 0; i < cantidad; i++)
            {
                carritoExistente.Flores.Add(flor);
            }

            // O mejor: si tu modelo lo permite, tener una propiedad Cantidad en una clase intermedia
            // carritoExistente.Items.Add(new ItemCarrito { Flor = flor, Cantidad = cantidad });

            HttpContext.Session.SetObjectAsJson("Carrito", carrito);

            return Ok(new
            {
                mensaje = $"Agregado {cantidad} unidad(es) de la flor {florId}",
                totalItems = carritoExistente.Flores.Count,
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
                return NotFound("El carrito está vacío.");
            }

            List<Carrito> productosCarritoCompleto = new List<Carrito>();
            //foreach (var producto in productosCarritoCompleto)
            //{
            //    Flor flor = new 

            //}

            return Ok(carritoExistente);
        }
    }
}
