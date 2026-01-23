namespace ServidorTiendaDotNet.DTOs
{
    public class PedidoDetalleResponse
    {
        public int FlorId{ get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
    }
}
