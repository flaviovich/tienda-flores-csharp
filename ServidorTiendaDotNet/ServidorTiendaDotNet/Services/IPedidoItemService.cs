using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface IPedidoItemService
    {
        Task<PedidoItem> GetByIdAsync(int id);

        // podría devolver cuántas filas se insertaron: Task<int>
        Task CreateRangeAsync(int pedidoId, IEnumerable<PedidoItemCreateDto> items);
        
        Task<List<PedidoItemResponse>> GetByPedidoIdAsync(int pedidoId);
        Task<int> GetCount();
        // Opcionales muy útiles en la práctica
        //Task<decimal> GetSubtotalByPedidoIdAsync(int pedidoId);
        //Task<bool> ExistsByPedidoIdAndFlorIdAsync(int pedidoId, int florId);
    }
}
