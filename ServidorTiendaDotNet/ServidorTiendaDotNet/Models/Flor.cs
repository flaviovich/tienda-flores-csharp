using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models
{
    public class Flor
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }
        [JsonPropertyName("activo")]
        public bool Activo { get; set; }
    }
}
