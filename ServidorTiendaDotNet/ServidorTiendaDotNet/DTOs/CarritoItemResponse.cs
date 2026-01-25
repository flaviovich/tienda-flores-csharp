namespace ServidorTiendaDotNet.DTOs
{
    public class CarritoItemResponse
    {
        public int FlorId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        //public string? ImagenUrl { get; set; }   // para mostrar foto en el frontend

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Cantidad * PrecioUnitario;

        // public string? Variedad { get; set; }
        // public bool Disponible { get; set; } = true;
        // public int StockDisponible { get; set; }
    }
}