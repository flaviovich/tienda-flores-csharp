using Microsoft.Data.Sqlite;
using System;
using System.Threading.Tasks;

namespace WebApplication4;

public static class DbTest
{
    private const string DbPath = @"C:\Users\admin\Desktop\Ares\pruebas_dot_net\libros.db";

    public static async Task Run()
    {
        Console.WriteLine("=== Prueba de conexión SQLite ===\n");

        try
        {
            await using var connection = new SqliteConnection($"Data Source={DbPath}");
            await connection.OpenAsync();
            Console.WriteLine("✓ Conexión establecida correctamente\n");

            // Crear tabla si no existe
            await using (var createCmd = connection.CreateCommand())
            {
                createCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS libros (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        titulo TEXT NOT NULL,
                        autor TEXT,
                        anio INTEGER
                    );";
                await createCmd.ExecuteNonQueryAsync();
                Console.WriteLine("✓ Tabla 'libros' verificada\n");
            }

            // Insertar registro de prueba si está vacía
            await using (var countCmd = connection.CreateCommand())
            {
                countCmd.CommandText = "SELECT COUNT(*) FROM libros;";
                var count = (long)(await countCmd.ExecuteScalarAsync() ?? 0L);

                if (count == 0)
                {
                    await using var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = @"
                        INSERT INTO libros (titulo, autor, anio) 
                        VALUES ('El Quijote', 'Miguel de Cervantes', 1605);";
                    await insertCmd.ExecuteNonQueryAsync();
                    Console.WriteLine("✓ Registro de prueba insertado\n");
                }
            }

            // Listar todos los libros
            Console.WriteLine("--- Listado de libros ---");
            await using (var selectCmd = connection.CreateCommand())
            {
                selectCmd.CommandText = "SELECT id, titulo, autor, anio FROM libros;";
                await using var reader = await selectCmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var id = reader.GetInt64(0);
                    var titulo = reader.IsDBNull(1) ? "(sin título)" : reader.GetString(1);
                    var autor = reader.IsDBNull(2) ? "(desconocido)" : reader.GetString(2);
                    var anio = reader.IsDBNull(3) ? "N/A" : reader.GetInt32(3).ToString();

                    Console.WriteLine($"  [{id}] {titulo} - {autor} ({anio})");
                }
            }

            Console.WriteLine("\n=== Prueba completada ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }
    }
}