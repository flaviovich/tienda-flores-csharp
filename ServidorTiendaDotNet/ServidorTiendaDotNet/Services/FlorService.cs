using Microsoft.Data.Sqlite;
using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public class FlorService : IFlorService
    {
        readonly SqliteConnection _connection;

        public FlorService(SqliteConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<FlorResponse>> GetAllAsync()
        {
            var flores = new List<FlorResponse>();

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT id, nombre, color, precio, stock, fecha_actualizacion " +
                "FROM flores " +
                "WHERE activo = 1;";
            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var flor = new FlorResponse();
                flor.Id = reader.GetInt32(0);
                flor.Nombre = reader.GetString(1);
                flor.Color = reader.GetString(2);
                flor.Precio = reader.GetDecimal(3);
                flor.Stock = reader.GetInt32(4);
                flor.FechaActualizacion = reader.IsDBNull(5) ? null : reader.GetString(5);
                flores.Add(flor);
            }

            return flores;
        }

        public async Task<FlorResponse?> GetByIdAsync(int id)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT id, nombre, color, precio, stock " +
                                  "FROM flores WHERE id = $id;";
            command.Parameters.AddWithValue("$id", id);
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var flor = new FlorResponse();
                flor.Id = reader.GetInt32(0);
                flor.Nombre = reader.GetString(1);
                flor.Color = reader.GetString(2);
                flor.Precio = reader.GetDecimal(3);
                flor.Stock = reader.GetInt32(4);

                return flor;
            }

            return null;
        }

        public async Task<FlorCreateDto> CreateAsync(FlorCreateDto dto)
        {
            var flor = new FlorCreateDto
            {
                Nombre = dto.Nombre,
                Color = dto.Color,
                Precio = dto.Precio,
                Stock = dto.Stock,
                //FechaCreacion = DateTime.Now, // o DateTime.UtcNow
            };

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            await using var transaction = (SqliteTransaction)await _connection.BeginTransactionAsync();

            try
            {
                await using var command = _connection.CreateCommand();
                command.Transaction = transaction;

                command.CommandText = @"
                    INSERT INTO flores (nombre, color, precio, stock)
                    VALUES ($nombre, $color, $precio, $stock);
                ";

                command.Parameters.AddWithValue("$nombre", flor.Nombre);
                command.Parameters.AddWithValue("$color", flor.Color);
                command.Parameters.AddWithValue("$precio", flor.Precio);
                command.Parameters.AddWithValue("$stock", flor.Stock);

                await command.ExecuteNonQueryAsync();

                // Obtener el ID generado
                command.CommandText = "SELECT last_insert_rowid();";
                var id = Convert.ToInt32(await command.ExecuteScalarAsync());

                flor.Id = id;

                await transaction.CommitAsync();

                return flor;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            // Verificar existencia del id
            await using (var checkCommand = _connection.CreateCommand())
            {
                checkCommand.CommandText = "SELECT COUNT(1) FROM flores WHERE id = $id;";
                checkCommand.Parameters.AddWithValue("$id", id);

                var result = await checkCommand.ExecuteScalarAsync();
                var count = Convert.ToInt32(result ?? 0);

                if (count == 0)
                {
                    return false; // El id no existe
                }
            }

            // Proceder a eliminar
            await using var command = _connection.CreateCommand();
            command.CommandText = "UPDATE flores SET activo = 0 WHERE id = $id;";
            command.Parameters.AddWithValue("$id", id);
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateAsync(int id, FlorUpdateDto flor)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            await using var command = _connection.CreateCommand();
            command.CommandText = @"
                UPDATE flores
                SET nombre = $nombre,
                    color = $color,
                    precio = $precio,
                    stock = $stock,
                    fecha_actualizacion = CURRENT_TIMESTAMP
                WHERE id = $id;
            ";
            command.Parameters.AddWithValue("$nombre", flor.Nombre);
            command.Parameters.AddWithValue("$color", flor.Color);
            command.Parameters.AddWithValue("$precio", flor.Precio);
            command.Parameters.AddWithValue("$stock", flor.Stock);
            command.Parameters.AddWithValue("$id", id);
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
    }
}