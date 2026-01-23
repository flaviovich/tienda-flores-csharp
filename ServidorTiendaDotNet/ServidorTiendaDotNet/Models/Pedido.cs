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

public enum MetodoPagoTipo
{
    Tarjeta,
    PayPal,
    Transferencia,
    ContraReembolso
}

public class Pedido
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
    [MinLength(2), MaxLength(120)]
    [JsonPropertyName("cliente")]
    public string Cliente { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\+?\d[\d\s-]{6,14}$")]
    [JsonPropertyName("telefono")]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(10), MaxLength(250)]
    [JsonPropertyName("direccionEnvio")]
    public string DireccionEnvio { get; set; } = string.Empty;

    [Required]
    public MetodoPagoTipo MetodoPago { get; set; } = MetodoPagoTipo.Tarjeta;
    
    [Required]
    [CreditCard]
    [StringLength(19, MinimumLength = 13)] // acepta espacios y diferentes longitudes
    public string NumeroTarjeta { get; set; } = string.Empty;
    
    // ── Campos de negocio esenciales ───────────────────────────────
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public PedidoEstado Estado { get; set; } = PedidoEstado.Pendiente;

    [Range(0.01, 999999.99)]
    public decimal Total { get; set; }

    public List<PedidoItem> Items { get; set; } = new();

    // Opcionales
    public DateTime? FechaUltimoCambio { get; set; }
    public string? Notas { get; set; }
    //public bool PagoConfirmado { get; set; }           // en vez de guardar tarjeta
    public DateTime? FechaPago { get; set; }
}