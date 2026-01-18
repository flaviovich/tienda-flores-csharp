namespace ServidorTiendaDotNet.Models
{
    public class AddCartItemDto
    {
        public int FlorId { get; set; }
        public int Cantidad { get; set; } = 1;
    }
}
