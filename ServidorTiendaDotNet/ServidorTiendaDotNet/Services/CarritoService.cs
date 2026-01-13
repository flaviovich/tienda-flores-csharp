using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public class CarritoService : ICarritoService
    {
        public CarritoService() { }

        public Task<Carrito> ObtenerCarritoAsync(int carritoId)
        {
            throw new NotImplementedException();
        }

        public Task AgregarFlorAlCarritoAsync(int carritoId, Flor flor)
        {
            throw new NotImplementedException();
        }

        // Implementación de métodos para manejar el carrito de compras

    }
}
