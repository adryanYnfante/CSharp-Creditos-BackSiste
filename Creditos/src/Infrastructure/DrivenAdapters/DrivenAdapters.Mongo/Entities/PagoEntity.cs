using System;

namespace DrivenAdapters.Mongo.Entities
{
    /// <summary>
    /// Entidad de pago para guardar en base de datos
    /// </summary>
    public class PagoEntity
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Monto del pago
        /// </summary>
        public decimal Monto { get; set; }

        /// <summary>
        /// Fecha en que se realizó el pago
        /// </summary>
        public DateTime FechaDeCancelacion { get; set; }
    }
}