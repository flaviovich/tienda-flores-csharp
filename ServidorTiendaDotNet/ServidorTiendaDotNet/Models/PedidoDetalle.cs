using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models
{
    public class PedidoDetalle
    {
        public int Id { get; set; }

        [JsonPropertyName("pedidoId")]
        public int PedidoId { get; set; }

        [JsonPropertyName("florId")]
        public int FlorId { get; set; }

        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }

        [JsonPropertyName("precioUnitario")]
        public decimal PrecioUnitario { get; set; }
    }
}
