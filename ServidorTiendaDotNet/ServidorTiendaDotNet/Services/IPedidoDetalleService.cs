using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface IPedidoDetalleService
    {
        Task<int> GetCount();
        Task<PedidoDetalleService> CreateAsync(PedidoDetalleService pedidoDetalle);
    }
}
