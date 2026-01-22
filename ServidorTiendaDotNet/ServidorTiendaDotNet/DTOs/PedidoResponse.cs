namespace ServidorTiendaDotNet.DTOs
{
    public class PedidoResponse
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public decimal Total { get; set; }

    }
}
