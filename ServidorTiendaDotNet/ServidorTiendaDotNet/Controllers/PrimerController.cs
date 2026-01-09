using Microsoft.AspNetCore.Mvc;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Controllers
{
    // En Java esto sería un @RestController
    [ApiController]

    // En Java esto sería un @RequestMapping("/primer")
    [Route("[controller]")]
    public class PrimerController : ControllerBase
    {
        private readonly ILogger<PrimerController> _logger;

        public PrimerController(ILogger<PrimerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("holamundo")]
        public string GetHolaMundo()
        {
            _logger.LogInformation("Se ha recibido una petición a /primer/holamundo");

            return "Hola Mundo desde .NET Core";
        }

        [HttpGet("flores")]
        public List<Flor> GetFlores()
        {
            _logger.LogInformation("Se ha recibido una petición a /primer/flores");
            var flores = new List<Flor>
            {
                new Flor
                {
                    Id = 1,
                    Nombre = "Rosa",
                    Descripcion = "Flor roja y fragante",
                    Precio = 2.5m,
                    Activo = true
                },
                new Flor
                {
                    Id = 2,
                    Nombre = "Tulipán",
                    Descripcion = "Flor colorida de primavera",
                    Precio = 1.8m,
                    Activo = true
                },
                new Flor
                {
                    Id = 3,
                    Nombre = "Margarita",
                    Descripcion = "Flor blanca con centro amarillo",
                    Precio = 1.2m,
                    Activo = false
                }
            };
            return flores;
        }
    }
}
