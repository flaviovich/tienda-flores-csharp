namespace ServidorTiendaDotNet.Models;

public class CarritoResponse
{
    public int Id { get; init; }
    public List<CarritoItemResponse> Items { get; init; } = new();
    public int CantidadItems => Items.Sum(i => i.Cantidad);
    public decimal Total => Items.Sum(i => i.Subtotal);
}