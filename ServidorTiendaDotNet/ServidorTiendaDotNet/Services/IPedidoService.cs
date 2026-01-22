using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface IPedidoService
    {
        Task<List<PedidoResponse>> GetAllAsync();
        Task<PedidoResponse> GetByIdAsync(int id);
        Task<PedidoResponse> CreateAsync(PedidoCreateDto pedido, CarritoResponse carrito);
        Task<int> GetCount();
    }
}
