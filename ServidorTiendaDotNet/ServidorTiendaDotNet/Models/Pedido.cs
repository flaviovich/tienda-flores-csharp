using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServidorTiendaDotNet.Models;

public enum PedidoEstado
{
    Pendiente,
    Pagado,
    EnPreparacion,
    Enviado,
    Entregado,
    Cancelado,
    Reembolsado
}

public class Pedido
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
    [MinLength(2, ErrorMessage = "Mínimo 2 caracteres")]
    [MaxLength(120)]
    [JsonPropertyName("cliente")]
    public string Cliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "Teléfono obligatorio")]
    [RegularExpression(@"^\+?\d[\d\s-]{6,14}$", ErrorMessage = "Formato de teléfono inválido")]
    [JsonPropertyName("telefono")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email obligatorio")]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Dirección de envío obligatoria")]
    [MinLength(10)]
    [MaxLength(250)]
    [JsonPropertyName("direccionEnvio")]
    public string DireccionEnvio { get; set; } = string.Empty;

    [Required]
    public string MetodoPago { get; set; } = string.Empty; // "stripe", "paypal", etc.
    
    [Required]
    [CreditCard]
    [StringLength(19, MinimumLength = 13)] // acepta espacios y diferentes longitudes
    public string NumeroTarjeta { get; set; } = string.Empty;
    
    // Solo si usas tokenización (recomendado)
    //public string? PaymentToken { get; set; }     // Stripe token / PaymentMethodId
    //public string? PaymentIdExterno { get; set; } // ID de transacción en Stripe/PayPal

    // ── Campos esenciales que faltaban ───────────────────────
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public PedidoEstado Estado { get; set; } = PedidoEstado.Pendiente;

    public decimal Total { get; set; }

    public DateTime? FechaUltimoCambio { get; set; }

    // Opcional: si tienes usuarios
    //public string? UserId { get; set; }

    // Relación con detalles (si usas EF)
    // public List<PedidoDetalle> Detalles { get; set; } = new();
}