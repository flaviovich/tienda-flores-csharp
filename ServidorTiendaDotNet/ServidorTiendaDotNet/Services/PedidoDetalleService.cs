
using Microsoft.Data.Sqlite;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public class PedidoDetalleService : IPedidoDetalleService
    {
        readonly SqliteConnection _connection;

        public async Task<PedidoDetalle> GetByIdAsync(int id)
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
                var item = new PedidoDetalle();
                item.Id = reader.GetInt32(0);
                item.PedidoId = reader.GetInt32(1);
                item.FlorId = reader.GetInt32(2);
                item.Cantidad = reader.GetInt32(3);
                item.PrecioUnitario = reader.GetInt32(4);

                return item;
            }

            return null;
        }

        public Task<PedidoDetalle> CreateAsync(PedidoDetalle pedidoDetalle)
        {
            throw new NotImplementedException();
        }
        
        public Task<int> GetCount()
        {
            throw new NotImplementedException();
        }
    }
}
