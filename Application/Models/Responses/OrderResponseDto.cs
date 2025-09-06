namespace Application.Dtos
{
    public class OrderResponseDto
    {
        public string ClienteNombre { get; set; }
        public string LibroTitulo { get; set; }
        public string CategoriaNombre { get; set; }
        public string AutorNombre { get; set; }
        public string AutorApellido { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}
