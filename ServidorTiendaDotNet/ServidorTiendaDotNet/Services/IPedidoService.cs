using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface IPedidoService
    {
        Task<List<PedidoResponse>> GetAllAsync();
        Task<PedidoResponse?> GetByIdAsync(int pedidoId);
        Task<PedidoResponse> CreateAsync(PedidoCreateDto pedidoDto, CarritoResponse carrito);
        //Task<int> GetCount();
    }
}
