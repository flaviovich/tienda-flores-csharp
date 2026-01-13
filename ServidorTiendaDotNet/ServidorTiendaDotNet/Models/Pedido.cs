using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [JsonPropertyName("cliente")]
        public String Cliente { get; set; }

        [JsonPropertyName("numeroTarjeta")]
        public string NumeroTarjeta { get; set; }

        [JsonPropertyName("direccionEnvio")]
        public string DireccionEnvio { get; set; } = string.Empty;

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }
    }
}
