namespace DrivenAdapters.ServiceBus.Entities
{
    public class PagoEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public decimal Monto { get; set; }

        public DateTime FechaDeCancelacion { get; set; } = DateTime.Now;

        public string Correo { get; set; }
    }
}