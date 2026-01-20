using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface IPedidoDetalleService
    {
        Task<PedidoDetalle> GetByIdAsync(int id);
        Task<PedidoDetalle> CreateAsync(PedidoDetalle pedidoDetalle);
        Task<int> GetCount();
    }
}
