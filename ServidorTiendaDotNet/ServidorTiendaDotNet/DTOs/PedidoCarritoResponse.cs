namespace ServidorTiendaDotNet.DTOs
{
    public class PedidoCarritoResponse
    {
        public PedidoResponse Pedido { get; set; }
        public List<FlorCarrito> Items { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}
