using System.Linq;
using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        public List<CarritoItem> Items { get; set; } = new List<CarritoItem>();
        public int CantidadItems => Items.Sum(i => i.Cantidad);
        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}