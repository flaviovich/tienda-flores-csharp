using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre no puede estar vacío")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ,. ]{3,20}$")]
        [JsonPropertyName("cliente")]
        public String Cliente { get; set; }

        
        [Required(ErrorMessage = "Tarjeta no puede estar vacio")]
        [RegularExpression("^[0-9 ]{16-22}$")]
        [JsonPropertyName("numeroTarjeta")]
        public string NumeroTarjeta { get; set; }

        [MinLength(10, ErrorMessage = "Dirección debe tener al menos 10 caracteres")]
        [MaxLength(100, ErrorMessage = "Dirección no puede tener más de 100 caracteres")]
        [JsonPropertyName("direccionEnvio")]
        public string DireccionEnvio { get; set; } = string.Empty;

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }
    }
}
