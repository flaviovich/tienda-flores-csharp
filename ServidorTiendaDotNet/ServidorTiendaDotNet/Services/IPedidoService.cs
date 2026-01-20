using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface IPedidoService
    {
        Task<List<Pedido>> GetAllAsync();
        Task<Pedido> GetByIdAsync(int id);
        Task<Pedido> CreateAsync(Pedido pedido, Carrito carrito);
        Task<int> GetCount();
    }
}
