using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models
{
    public class PedidoItem
    {
        public int Id { get; set; }

        [JsonPropertyName("pedidoId")]
        public int PedidoId { get; set; }

        [JsonPropertyName("florId")]
        public int FlorId { get; set; }

        [Range(1, 9999, ErrorMessage = "La cantidad debe estar entre 1 y 9999")]
        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }

        [Range(0.01, 99999.99, ErrorMessage = "Precio unitario fuera de rango")]
        [JsonPropertyName("precioUnitario")]
        public decimal PrecioUnitario { get; set; }

        // Opcional pero muy útil en la mayoría de proyectos
        [JsonPropertyName("subtotal")]
        public decimal Subtotal => Cantidad * PrecioUnitario;

        // Si en el futuro necesitas guardar descuentos por línea
        // public decimal? DescuentoPorcentaje { get; set; }
        // public decimal? DescuentoImporte    { get; set; }
    }    
}
