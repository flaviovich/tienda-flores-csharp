namespace ServidorTiendaDotNet.DTOs
{
    public class FlorUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
