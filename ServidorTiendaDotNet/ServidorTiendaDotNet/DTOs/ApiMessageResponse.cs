namespace ServidorTiendaDotNet.DTOs
{
    public record ApiMessageResponse
    {
        public string Mensaje { get; init; } = string.Empty;
        public int TotalItems { get; init; }
        public decimal Total { get; init; }
    }
}