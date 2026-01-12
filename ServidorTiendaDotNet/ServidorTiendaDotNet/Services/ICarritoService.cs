using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface ICarritoService
    {
        Task<Carrito> ObtenerCarritoAsync(int carritoId);
        Task AgregarFlorAlCarritoAsync(int carritoId, Flor flor);
    }
}
