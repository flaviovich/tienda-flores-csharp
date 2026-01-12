using Microsoft.Data.Sqlite;
using ServidorTiendaDotNet.Models;
using ServidorTiendaDotNet.Repository;

namespace ServidorTiendaDotNet.Repositories
{
    public class TiendaRepository : ITiendaRepository
    {
        private const string DbPath = @"C:\Users\frios\DAW2\ws_vstudio\ServidorTiendaDotNet\ServidorTiendaDotNet\DB\bd_tienda_flores.db";

        private SqliteConnection ConnectToDatabase()
        {
            var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            return connection;
        }

        public int AddFlor(Flor nuevaFlor)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFlor(int id)
        {
            throw new NotImplementedException();
        }

        public Flor GetFlorById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Flor>> GetFlores()
        {
            var flores = new List<Flor>();
            using var connection = ConnectToDatabase();
            await using (var selectCmd = connection.CreateCommand())
            {
                selectCmd.CommandText = "SELECT id, nombre, descripcion, precio, activo FROM flores;";
                await using var reader = await selectCmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var flor = new Flor
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Color = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Precio = reader.GetDecimal(3),
                        EnStock = reader.GetBoolean(4)
                    };
                    flores.Add(flor);
                }
            }

            return flores;
        }

        public bool UpdateFlor(int id, Flor florActualizada)
        {
            throw new NotImplementedException();
        }

    }
}
