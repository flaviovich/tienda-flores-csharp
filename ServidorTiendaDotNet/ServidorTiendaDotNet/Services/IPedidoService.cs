using ServidorTiendaDotNet.DTOs;

namespace ServidorTiendaDotNet.Services
{
    public interface IPedidoService
    {
        Task<List<PedidoResponse>> GetAllAsync();
        Task<PedidoResponse?> GetByIdAsync(int pedidoId);
        Task<PedidoResponse> PedidoCreateAsync(PedidoCreateDto pedidoDto, List<CarritoItemDto> carrito);
        Task<PedidoCarritoResponse> GetPedidoItemByIdAsync(int pedidoId);
        //Task<int> GetCount();
        Task<List<FlorCarrito>> GetPedidoDetalleByIdAsync(int pedidoId);
    }
}
