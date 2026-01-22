using System.ComponentModel.DataAnnotations;

namespace ServidorTiendaDotNet.DTOs;

public class PedidoCreateDto
{
    // Datos personales / envío
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MinLength(2)]
    [MaxLength(100)]
    public string Cliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "Teléfono es obligatorio")]
    [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Formato de teléfono inválido")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email es obligatorio")]
    [EmailAddress(ErrorMessage = "Email no válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección de envío es obligatoria")]
    [MinLength(10, ErrorMessage = "La dirección debe tener al menos 10 caracteres")]
    [MaxLength(150)]
    public string DireccionEnvio { get; set; } = string.Empty;

    // ── Pago ───────────────────────────────────────────────
    [Required(ErrorMessage = "Se requiere un método de pago")]
    public string MetodoPago { get; set; } = "stripe"; // o "paypal", "tarjeta", "transferencia", etc.
    
    [Required]
    [CreditCard]
    [StringLength(19, MinimumLength = 13)] // acepta espacios y diferentes longitudes
    public string NumeroTarjeta { get; set; } = string.Empty;
}