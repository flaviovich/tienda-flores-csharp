using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface IPedidoService
    {
        Task<int> GetCount();
        Task<Pedido> CreateAsync(Pedido pedido, Carrito carrito);
    }
}
