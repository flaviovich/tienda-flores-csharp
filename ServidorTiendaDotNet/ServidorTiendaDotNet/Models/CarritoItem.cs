using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models
{
    public class CarritoItem
    {
        [JsonPropertyName("florId")] public int FlorId { get; set; }

        [JsonPropertyName("flor")] public FlorResponse? Flor { get; set; }

        [JsonPropertyName("cantidad")] public int Cantidad { get; set; } = 1;

        [JsonPropertyName("precioUnitario")] public decimal PrecioUnitario { get; set; }

        [JsonPropertyName("subtotal")] public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}