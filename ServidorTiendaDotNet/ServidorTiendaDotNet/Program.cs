using ServidorTiendaDotNet.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type =>
    {
        // Quita el sufijo "DTO" si existe
        var name = type.Name;
        if (name.EndsWith("DTO"))
        {
            return name[..^3];  // quita las últimas 3 letras ("DTO")
        }
        return name;
    });
});

// Configuración de sesiones
builder.Services.AddDataProtection();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Inyección de dependencia para la conexión a SQLite
builder.Services.AddScoped(sp =>
{
    var connString = builder.Configuration.GetConnectionString("DefaultSQLite")
                     ?? "Data Source=Data/localdb.db";

    //var conn = new Microsoft.Data.Sqlite.SqliteConnection(connString);
    
    return new Microsoft.Data.Sqlite.SqliteConnection(connString);
});

// Inyección de dependencia para el servicio de flores
builder.Services.AddScoped<IFlorService, FlorService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<ICarritoService, CarritoService>();
builder.Services.AddScoped<IPedidoDetalleService, PedidoDetalleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar el uso de sesiones
app.UseSession();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
