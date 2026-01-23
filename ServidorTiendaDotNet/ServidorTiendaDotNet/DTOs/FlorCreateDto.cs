using System.ComponentModel.DataAnnotations;

namespace ServidorTiendaDotNet.Models
{
    public class FlorCreateDto
    {
        public int Id { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Color { get; set; } = string.Empty;

        /// <summary>Precio en euros. Usa punto como separador decimal.</summary>
        /// <example>12.34</example>
        [Required, Range(0.01d, 9999.99d)]
        public decimal Precio { get; set; }

        [Required] public int Stock { get; set; } = 1;

        public IFormFile? Imagen { get; set; }
    }
}
