namespace ServidorTiendaDotNet.DTOs
{
    public class PedidoResponse
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NumeroTarjeta { get; set; } = string.Empty;
        public string DireccionEnvio { get; set; } = string.Empty;

    }
}
