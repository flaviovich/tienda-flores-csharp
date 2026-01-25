using Microsoft.Data.Sqlite;
using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public class PedidoService : IPedidoService
    {
        readonly SqliteConnection _connection;
        readonly IFlorService _florService;
        readonly ILogger<PedidoService> _logger;

        public PedidoService(SqliteConnection connection, IFlorService florService, ILogger<PedidoService> logger)
        {
            _connection = connection;
            _florService = florService;
            _logger = logger;
        }

        public async Task<List<PedidoResponse>> GetAllAsync()
        {
            var pedidos = new List<PedidoResponse>();

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT id, nombre_cliente, telefono, fecha, estado, total " +
                                  "FROM pedidos;";
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var item = new PedidoResponse();
                item.Id = reader.GetInt32(0);
                item.Cliente = reader.GetString(1);
                item.Telefono = reader.GetString(2);
                item.Fecha = reader.GetDateTime(3);
                item.Estado = reader.GetString(4);
                item.Total = reader.GetDecimal(5);
                pedidos.Add(item);
            }

            return pedidos;
        }

        public async Task<PedidoCarritoResponse> GetPedidoItemByIdAsync(int pedidoId)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using var command = _connection.CreateCommand();

            var pedido = await GetByIdAsync(pedidoId);
            var detalle = await GetPedidoDetalleByIdAsync(pedidoId);

            command.CommandText = @"
                SELECT sum(cantidad)cantidad, (cantidad*precio_unitario)total
                FROM pedido_detalles 
                WHERE pedido_id = $pedidoId;
            ";
            command.Parameters.AddWithValue("$pedidoId", pedidoId);
            await using var reader = await command.ExecuteReaderAsync();

            int cantidad = 0;
            decimal total = 0;
            if (reader.Read())
            {
                cantidad = reader.GetInt32(0);
                total = reader.GetDecimal(1);
            }

            var pedidoCarritoResponse = new PedidoCarritoResponse
            {
                Pedido = pedido!,
                Items = detalle,
                Cantidad = (int)cantidad,
                Total = total
            };
            return pedidoCarritoResponse;
        }

        public async Task<PedidoResponse?> GetByIdAsync(int pedidoId)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT id, nombre_cliente, telefono, fecha, estado, total " +
                                  "FROM pedidos WHERE id = $id;";
            command.Parameters.AddWithValue("$id", pedidoId);
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var item = new PedidoResponse();
                item.Id = reader.GetInt32(0);
                item.Cliente = reader.GetString(1);
                item.Telefono = reader.GetString(2);
                item.Fecha = reader.GetDateTime(3);
                item.Estado = reader.GetString(4);
                item.Total = reader.GetDecimal(5);
                return item;
            }

            return null;
        }

        public async Task<PedidoResponse> PedidoCreateAsync(PedidoCreateDto pedido, List<CarritoItemDto> carrito)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            var pedidoNew = new Pedido
            {
                Cliente = pedido.Cliente,
                Telefono = pedido.Telefono,
                Email = pedido.Email,
                DireccionEnvio = pedido.DireccionEnvio,
                MetodoPago = Enum.TryParse<MetodoPagoTipo>(pedido.MetodoPago, true, out var metodo)
                    ? metodo
                    : MetodoPagoTipo.Tarjeta, // fallback,
                NumeroTarjeta = pedido.NumeroTarjeta,
                Tipo = pedido.Tipo,
                Notas = pedido.NotasEnvio,
            };

            decimal subtotal;
            decimal total = 0;
            using var transaction = await _connection.BeginTransactionAsync();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO pedidos (nombre_cliente, telefono, email, direccion_envio, codigo_postal, ciudad,
                                         numero_tarjeta, notas_envio, tipo, metodo_pago)
                    VALUES ($cliente, $telefono, $email, $direccion_envio, $codigo_postal, $ciudad,
                            $numero_tarjeta, $notas_envio, $tipo, $metodo_pago);
                ";

                command.Parameters.AddWithValue("$cliente", pedido.Cliente);
                command.Parameters.AddWithValue("$telefono", pedido.Telefono);
                command.Parameters.AddWithValue("$email", pedido.Email);
                command.Parameters.AddWithValue("$direccion_envio", pedido.DireccionEnvio);
                command.Parameters.AddWithValue("codigo_postal", pedido.CodigoPostal);
                command.Parameters.AddWithValue("ciudad", pedido.Ciudad);
                command.Parameters.AddWithValue("$numero_tarjeta", pedido.NumeroTarjeta);
                command.Parameters.AddWithValue("notas_envio", pedido.NotasEnvio);
                command.Parameters.AddWithValue("tipo", pedido.Tipo);
                command.Parameters.AddWithValue("metodo_pago", pedido.MetodoPago);
                await command.ExecuteNonQueryAsync();

                // Obtener el ID del último registro insertado usando una consulta SQL
                using (var lastIdCommand = _connection.CreateCommand())
                {
                    lastIdCommand.CommandText = "SELECT last_insert_rowid();";
                    var result = await lastIdCommand.ExecuteScalarAsync();
                    pedidoNew.Id = Convert.ToInt32(result);
                }
                
                // Insertar los detalles del pedido y calcular el total
                foreach (var item in carrito)
                {
                    var flor = await _florService.GetByIdAsync(item.FlorId);
                    subtotal = item.Cantidad * flor.Precio;
                    total += subtotal;
                    
                    using var detalleCommand = _connection.CreateCommand();
                    detalleCommand.CommandText = @"
                        INSERT INTO pedido_detalles (pedido_id, flor_id, cantidad, precio_unitario, subtotal)
                        VALUES ($pedido_id, $flor_id, $cantidad, $precio_unitario, $subtotal);
                    ";
                    detalleCommand.Parameters.AddWithValue("$pedido_id", pedidoNew.Id);
                    detalleCommand.Parameters.AddWithValue("$flor_id", item.FlorId);
                    detalleCommand.Parameters.AddWithValue("$cantidad", item.Cantidad);
                    detalleCommand.Parameters.AddWithValue("$precio_unitario", flor.Precio);
                    detalleCommand.Parameters.AddWithValue("$subtotal", subtotal);
                    await detalleCommand.ExecuteNonQueryAsync();
                }

                // Redondeo a 2 decimales (estándar monetario)
                total = Math.Round(total, 2, MidpointRounding.AwayFromZero);

                // Actualizar el total del pedido
                using var updatePedidoCommand = _connection.CreateCommand();
                updatePedidoCommand.CommandText = @"
                    UPDATE pedidos
                    SET total = $total
                    WHERE id = $pedido_id;
                ";
                updatePedidoCommand.Parameters.AddWithValue("$total", total);
                updatePedidoCommand.Parameters.AddWithValue("$pedido_id", pedidoNew.Id);
                await updatePedidoCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

            // Mapear Pedido → PedidoResponse
            var response = new PedidoResponse
            {
                Id = pedidoNew.Id,
                Cliente = pedidoNew.Cliente,
                Telefono = pedidoNew.Telefono,
                Fecha = pedidoNew.FechaCreacion,
                Estado = pedidoNew.Estado.ToString(),
                Total = total,
            };

            return response;
        }

        public async Task<List<FlorCarrito>> GetPedidoDetalleByIdAsync(int pedidoId)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            var detalles = new List<FlorCarrito>();
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                SELECT flor_id, cantidad
                FROM pedido_detalles
                WHERE pedido_id = $pedidoId;
            ";
            command.Parameters.AddWithValue("$pedidoId", pedidoId);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var flor = await _florService.GetByIdAsync(reader.GetInt32(0));
                var detalle = new FlorCarrito
                {
                    Flor = flor,
                    Cantidad = reader.GetInt32(1),
                };
                detalles.Add(detalle);
            }

            return detalles;
        }
    }
}