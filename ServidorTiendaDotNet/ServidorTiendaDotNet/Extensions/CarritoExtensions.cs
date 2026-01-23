using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Extensions
{
    public static class CarritoExtensions
    {
        public static CarritoResponse ToResponse(this Carrito carrito)
        {
            ArgumentNullException.ThrowIfNull(carrito);

            return new CarritoResponse
            {
                Id = carrito.Id,
                Items = carrito.Items.Select(item => new CarritoItemResponse
                {
                    FlorId = item.FlorId,
                    NombreFlor = item.Flor?.Nombre ?? "[Flor no cargada]",
                    PrecioUnitario = item.PrecioUnitario,
                    Cantidad = item.Cantidad
                }).ToList()
            };
        }
    }    
}
