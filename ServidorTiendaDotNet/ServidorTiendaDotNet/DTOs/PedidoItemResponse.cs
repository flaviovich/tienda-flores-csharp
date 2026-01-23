namespace ServidorTiendaDotNet.DTOs
{
    public class PedidoItemResponse
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int FlorId { get; set; }
        public string NombreFlor { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        // Opcionales frecuentes
        //public string? Referencia  { get; set; }
        //public decimal? Descuento  { get; set; }
    }
}