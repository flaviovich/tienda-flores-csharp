namespace ServidorTiendaDotNet.DTOs
{
    public class CarritoResponse
    {
        public List<CarritoItemResponse> Items { get; set; } = new List<CarritoItemResponse>();

        public int TotalItems { get; set; }

        //public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

        public string? Mensaje { get; set; }

        // public decimal? CosteEnvio { get; set; }
        // public decimal? DescuentoAplicado { get; set; }
        // public string? Moneda { get; set; } = "EUR";
        // public bool TieneProductosAgotados { get; set; }
        // public DateTime UltimaActualizacion { get; set; }
    }
}
