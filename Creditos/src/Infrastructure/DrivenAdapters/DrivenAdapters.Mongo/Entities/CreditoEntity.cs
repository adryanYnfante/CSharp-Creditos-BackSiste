using DrivenAdapters.Mongo.Entities.Base;
using System;
using System.Collections.Generic;

namespace DrivenAdapters.Mongo.Entities
{
    /// <summary>
    /// Entidad de crédito en la base de datos
    /// </summary>
    public class CreditoEntity : EntityBase
    {
        /// <summary>
        /// Cantidad de dinero solicitada
        /// </summary>
        public decimal Monto { get; set; }

        /// <summary>
        /// Meses en los que se pagará el crédito
        /// </summary>
        public int PlazoEnMeses { get; set; }

        /// <summary>
        /// Cuotas pagadas hasta el momento
        /// </summary>
        public int CuotasPagadas { get; set; }

        /// <summary>
        /// Tasa de interés del crédito
        /// </summary>
        public decimal TasaDeInteres { get; set; }

        /// <summary>
        /// Total de intereses pagados
        /// </summary>
        public decimal TotalIntereses { get; set; }

        /// <summary>
        /// Monto de cada cuota
        /// </summary>
        public decimal MontoPorCuota { get; set; }

        /// <summary>
        /// Cuotas que faltan por pagar
        /// </summary>
        public int CuotasRestantes { get; set; }

        /// <summary>
        /// Fecha en la que se solicitó el crédito
        /// </summary>
        public DateTime FechaDeSolicitud { get; set; }

        /// <summary>
        /// Historial de pagos
        /// </summary>
        public List<PagoEntity> Pagos { get; set; }
    }
}