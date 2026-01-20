using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface IPedidoService
    {
        Task<List<Pedido>> GetAllAsync();
        Task<Pedido> GetByIdAsync(int id);
        Task<PedidoResponse> CreateAsync(PedidoCreateDto pedido, CarritoResponse carrito);
        Task<int> GetCount();
    }
}
