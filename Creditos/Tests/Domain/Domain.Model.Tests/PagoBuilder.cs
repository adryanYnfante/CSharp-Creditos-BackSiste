using Domain.Model.Entities;

namespace Domain.Model.Tests
{
    public class PagoBuilder
    {
        private readonly Pago _pago;

        public PagoBuilder()
        {
            _pago = new Pago();
        }

        public PagoBuilder WithId(string id)
        {
            _pago.Id = id;
            return this;
        }

        public PagoBuilder WithMonto(decimal monto)
        {
            _pago.Monto = monto;
            return this;
        }

        public PagoBuilder WithFechaDeCancelacion(DateTime fechaDeCancelacion)
        {
            _pago.FechaDeCancelacion = fechaDeCancelacion;
            return this;
        }

        public Pago Build()
        {
            return _pago;
        }
    }
}