namespace ServidorTiendaDotNet.Models;

public class CarritoItemResponse
{
    public int FlorId { get; init; }
    public string NombreFlor { get; init; } = string.Empty;
    public decimal PrecioUnitario { get; init; }
    public int Cantidad { get; init; }
    public decimal Subtotal => PrecioUnitario * Cantidad;
}