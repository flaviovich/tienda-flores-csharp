using Microsoft.AspNetCore.Mvc;
using ServidorTiendaDotNet.Services;

namespace ServidorTiendaDotNet.Controllers
{
    // En Java esto sería un @RestController
    [ApiController]

    // En Java esto ser�a un @RequestMapping("/primer")
    [Route("api/[controller]")]
    public class FloresController : ControllerBase
    {
        readonly ILogger<FloresController> _logger;
        readonly IFlorService _florService;

        public FloresController(ILogger<FloresController> logger, IFlorService florService)
        {
            _logger = logger;
            _florService = florService;
        }

        //[HttpGet("holamundo")]
        //public string GetHolaMundo()
        //{
        //    _logger.LogInformation("Se ha recibido una petición a /api/flores/holamundo");

        //    return "Hola Mundo desde .NET Core";
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Se ha recibido una petici�n a /api/flores");
            var flores = await _florService.GetAllAsync();

            return Ok(flores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var flor = await _florService.GetByIdAsync(id);

            if (flor == null)
                return NotFound();

            return Ok(flor);
        }
    }
}