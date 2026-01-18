namespace ServidorTiendaDotNet.Models;

public class FlorResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}