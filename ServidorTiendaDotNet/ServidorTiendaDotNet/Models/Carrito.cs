using System.Linq;
using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models
{
    public class Carrito
    {
        public int Id { get; set; }

        [JsonPropertyName("items")]
        public List<CarritoItem> Items { get; set; } = new List<CarritoItem>();

        [JsonPropertyName("cantidadItems")]
        public int CantidadItems => Items.Sum(i => i.Cantidad);

        [JsonPropertyName("total")]
        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}