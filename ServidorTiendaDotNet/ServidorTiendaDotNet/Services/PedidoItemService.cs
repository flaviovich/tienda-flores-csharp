using Microsoft.Data.Sqlite;
using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public class PedidoItemService : IPedidoItemService
    {
        readonly SqliteConnection _connection;

        public async Task<PedidoItem> GetByIdAsync(int id)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT pedido_id, flor_id, cantidad, precio_unitario " +
                "FROM pedido_detalles " +
                "WHERE pedido_id = $id;";
            command.Parameters.AddWithValue("id", id);
            await using var reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                var item = new PedidoItem();
                item.Id = reader.GetInt32(0);
                item.PedidoId = reader.GetInt32(1);
                item.FlorId = reader.GetInt32(2);
                item.Cantidad = reader.GetInt32(3);
                item.PrecioUnitario = reader.GetInt32(4);

                return item;
            }

            return null;
        }

        public async Task CreateRangeAsync(int pedidoId, IEnumerable<PedidoItemCreateDto> items)
        {
            if (!items.Any()) return;

            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync();

            using var transaction = await _connection.BeginTransactionAsync();

            decimal totalPedido = 0;
            
            foreach (var dto in items)
            {
                decimal precioReal = await ObtenerPrecioFlorAsync(dto.FlorId);
                
                if (precioReal <= 0)
                    throw new Exception($"Flor {dto.FlorId} no encontrada o sin precio");

                var entity = new PedidoItem
                {
                    PedidoId       = pedidoId,
                    FlorId         = dto.FlorId,
                    Cantidad       = dto.Cantidad,
                    PrecioUnitario = precioReal
                };
                
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO pedido_detalles (pedido_id, flor_id, cantidad, precio_unitario)
                VALUES ($p, $f, $c, $pu)";
            
                cmd.Parameters.AddWithValue("$p", pedidoId);
                cmd.Parameters.AddWithValue("$f", dto.FlorId);
                cmd.Parameters.AddWithValue("$c", dto.Cantidad);
                cmd.Parameters.AddWithValue("$pu", precioReal);
                await cmd.ExecuteNonQueryAsync();
                
                totalPedido += dto.Cantidad * precioReal;
            }

            await transaction.CommitAsync();
        }

        public async Task<List<PedidoItemResponse>> GetByPedidoIdAsync(int pedidoId)
        {
            var items = new List<PedidoItemResponse>();

            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync();

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
            SELECT id, pedido_id, flor_id, cantidad, precio_unitario
            FROM pedido_detalles
            WHERE pedido_id = $pedidoId
            ORDER BY id";
        
            cmd.Parameters.AddWithValue("$pedidoId", pedidoId);
        
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                items.Add(new PedidoItemResponse
                {
                    Id = reader.GetInt32(0),
                    PedidoId = reader.GetInt32(1),
                    FlorId = reader.GetInt32(2),
                    Cantidad = reader.GetInt32(3),
                    PrecioUnitario = reader.GetDecimal(4) 
                });
            }

            return items;
        }

        public Task<int> GetCount()
        {
            throw new NotImplementedException();
        }
        //Task<decimal> GetSubtotalByPedidoIdAsync(int pedidoId);
        //Task<bool> ExistsByPedidoAndFlorAsync(int pedidoId, int florId);
        //Task DeleteByPedidoAndFlorAsync(int pedidoId, int florId);
        
        private async Task<decimal> ObtenerPrecioFlorAsync(int florId)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT precio FROM flores WHERE id = $id;";
            cmd.Parameters.AddWithValue("$id", florId);

            var result = await cmd.ExecuteScalarAsync();
            return result != null && result != DBNull.Value 
                ? Convert.ToDecimal(result) 
                : 0m;
        }
    }
}
