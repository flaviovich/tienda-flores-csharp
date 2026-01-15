using Microsoft.Data.Sqlite;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public class FlorService : IFlorService
    {
        private readonly SqliteConnection _connection;

        public FlorService(SqliteConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<Flor>> GetAllAsync()
        {
            var flores = new List<Flor>();

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT id, nombre, color, precio, en_stock, creado_en FROM flores;";
            await using var reader = await command.ExecuteReaderAsync();

            while (
                reader.Read())
            {
                var flor = new Flor();
                flor.Id = reader.GetInt32(0);
                flor.Nombre = reader.GetString(1);
                flor.Color = reader.GetString(2);
                flor.Precio = reader.GetDecimal(3);
                flor.EnStock = reader.GetBoolean(4);
                //flor.FechaIngreso = reader.GetDateTime(5);
                flores.Add(flor);
            }

            return flores;
        }

        public async Task<Flor?> GetByIdAsync(int id)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT id, nombre, color, precio, en_stock, creado_en FROM flores WHERE id = $id;";
            command.Parameters.AddWithValue("$id", id);
            await using var reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                var flor = new Flor();
                flor.Id = reader.GetInt32(0);
                flor.Nombre = reader.GetString(1);
                flor.Color = reader.GetString(2);
                flor.Precio = reader.GetDecimal(3);
                flor.EnStock = reader.GetBoolean(4);
                //flor.FechaIngreso = reader.GetDateTime(5);

                return flor;
            }

            return null;
        }

        public Task<Flor> CreateAsync(Flor flor)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            using var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO flores (nombre, color, precio, en_stock) VALUES ($nombre, $color, $precio, $en_stock);";
            command.Parameters.AddWithValue("$nombre", flor.Nombre);
            command.Parameters.AddWithValue("$color", flor.Color);
            command.Parameters.AddWithValue("$precio", flor.Precio);
            command.Parameters.AddWithValue("$en_stock", flor.EnStock);
            command.ExecuteNonQuery();

            command.CommandText="SELECT last_insert_rowid();";
            var id = (int)command.ExecuteScalar()!;
            flor.Id = (int)id;

            return Task.FromResult(flor);
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, Flor flor)
        {
            throw new NotImplementedException();
        }
    }
}
