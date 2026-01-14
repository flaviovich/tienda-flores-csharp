using Microsoft.Data.Sqlite;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public class PedidoService : IPedidoService
    {
        readonly SqliteConnection _connection;

        public PedidoService(SqliteConnection connection)
        {
            _connection = connection;
        }

        public async Task<Pedido> CreateAsync(Pedido pedido, Carrito carrito)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using var transaction = await _connection.BeginTransactionAsync();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO pedidos (nombre_cliente, numero_tarjeta, direccion_envio, fecha)
                    VALUES ($cliente, $numero_tarjeta, $direccion_envio, $fecha);
                ";
                
                command.Parameters.AddWithValue("$cliente", pedido.Cliente);
                command.Parameters.AddWithValue("$numero_tarjeta", pedido.NumeroTarjeta);
                command.Parameters.AddWithValue("$direccion_envio", pedido.DireccionEnvio);
                command.Parameters.AddWithValue("$fecha", pedido.Fecha);
                await command.ExecuteNonQueryAsync();

                // Obtener el ID del último registro insertado usando una consulta SQL
                using (var lastIdCommand = _connection.CreateCommand())
                {
                    lastIdCommand.CommandText = "SELECT last_insert_rowid();";
                    var result = await lastIdCommand.ExecuteScalarAsync();
                    pedido.Id = Convert.ToInt32(result);
                }

                // Insertar los detalles del pedido
                foreach (var item in carrito.Items)
                {
                    using var detalleCommand = _connection.CreateCommand();
                    detalleCommand.CommandText = @"
                        INSERT INTO pedido_detalles (pedido_id, flor_id, cantidad, precio_unitario)
                        VALUES ($pedido_id, $flor_id, $cantidad, $precio_unitario);
                    ";
                    detalleCommand.Parameters.AddWithValue("$pedido_id", pedido.Id);
                    detalleCommand.Parameters.AddWithValue("$flor_id", item.Flor.Id);
                    detalleCommand.Parameters.AddWithValue("$cantidad", item.Cantidad);
                    detalleCommand.Parameters.AddWithValue("$precio_unitario", item.Flor.Precio);
                    await detalleCommand.ExecuteNonQueryAsync();
                }
            }
            await transaction.CommitAsync();

            return pedido;
        }

        Task<int> IPedidoService.GetCount()
        {
            throw new NotImplementedException();
        }
    }
}