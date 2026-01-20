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

        public async Task<List<Pedido>> GetAllAsync()
        {
            var pedidos = new List<Pedido>();

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT id, fecha, nombre_cliente, direccion_envio, telefono, email, " +
                "numero_tarjeta, estado " +
                "FROM pedidos;";
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var item = new Pedido();
                item.Id = reader.GetInt32(0);
                item.Cliente = reader.GetString(1);
                item.Telefono = reader.GetString(2);
                item.Email = reader.GetString(3);
                item.NumeroTarjeta = reader.GetString(4);
                item.DireccionEnvio = reader.GetString(5);
                pedidos.Add(item);
            }

            return pedidos;
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
                    INSERT INTO pedidos (nombre_cliente, direccion_envio, telefono, email, numero_tarjeta)
                    VALUES ($cliente, $direccion_envio, $telefono, $email, $numero_tarjeta);
                ";

                command.Parameters.AddWithValue("$cliente", pedido.Cliente);
                command.Parameters.AddWithValue("$direccion_envio", pedido.DireccionEnvio);
                command.Parameters.AddWithValue("$telefono", pedido.Telefono);
                command.Parameters.AddWithValue("$email", pedido.Email);
                command.Parameters.AddWithValue("$numero_tarjeta", pedido.NumeroTarjeta);
                await command.ExecuteNonQueryAsync();

                // Obtener el ID del último registro insertado usando una consulta SQL
                using (var lastIdCommand = _connection.CreateCommand())
                {
                    lastIdCommand.CommandText = "SELECT last_insert_rowid();";
                    var result = await lastIdCommand.ExecuteScalarAsync();
                    pedido.Id = Convert.ToInt32(result);
                }

                // Insertar los detalles del pedido y calcular el total
                decimal total = 0;
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

                    total += item.Cantidad * item.Flor.Precio;
                }

                // Actualizar el total del pedido
                using var updatePedidoCommand = _connection.CreateCommand();
                updatePedidoCommand.CommandText = @"
                    UPDATE pedidos
                    SET total = $total
                    WHERE id = $pedido_id;
                ";
                updatePedidoCommand.Parameters.AddWithValue("$total", total);
                updatePedidoCommand.Parameters.AddWithValue("$pedido_id", pedido.Id);
                await updatePedidoCommand.ExecuteNonQueryAsync();
            }
            await transaction.CommitAsync();

            return pedido;
        }

        Task<int> IPedidoService.GetCount()
        {
            throw new NotImplementedException();
        }

        public Task<Pedido> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}