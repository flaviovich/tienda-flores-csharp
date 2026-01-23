using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.DTOs
{
    public class PedidoItemCreateDto
    {
        [JsonPropertyName("florId")]
        [Range(1, int.MaxValue)]
        public int FlorId { get; set; }

        [Range(1, 9999)]
        public int Cantidad { get; set; }

    }
}