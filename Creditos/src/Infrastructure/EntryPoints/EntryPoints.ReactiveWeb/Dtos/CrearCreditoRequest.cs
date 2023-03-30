namespace EntryPoints.ReactiveWeb.Dtos
{
    public class CrearCreditoRequest
    {
        public decimal Monto { get; set; }

        public int PlazoEnMeses { get; set; }

        public string numeroDeDocumento { get; set; }
    }
}