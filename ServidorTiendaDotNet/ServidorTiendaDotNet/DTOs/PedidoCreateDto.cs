namespace ServidorTiendaDotNet.Models;

public class PedidoCreateDto
{
    public string Cliente { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NumeroTarjeta { get; set; } = string.Empty;
    public string DireccionEnvio { get; set; } = string.Empty;

}