using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models
{
    public class Carrito
    {
        public int Id { get; set; }

        [JsonPropertyName("flores")]        
        public List<Flor> Flores { get; set; } = new List<Flor>();

        [JsonPropertyName("cantidadItems")]
        public int CantidadItems => Flores.Count;

    }
}
