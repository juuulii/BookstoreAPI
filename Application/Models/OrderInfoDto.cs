namespace Application.Dtos
{
    public class OrderInfoDto
    {
        public int ClienteId { get; set; }
        public string? ClienteNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioTotal { get; set; }
        public DateTime Fecha { get; set; }
    }
}
