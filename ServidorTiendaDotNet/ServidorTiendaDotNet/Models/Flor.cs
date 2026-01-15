using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models
{
    public class Flor
    {
        //[JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("color")]
        public string Color { get; set; } = string.Empty;

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        [JsonPropertyName("enStock")]
        public bool EnStock { get; set; }

        [JsonIgnore]
        public IFormFile? Imagen { get; set; }

        //[JsonPropertyName("fechaIngreso")]
        //public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
    }
}