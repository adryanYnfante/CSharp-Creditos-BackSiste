using credinet.exception.middleware.models;
using Helpers.Commons.Exceptions;
using System;
using System.Collections.Generic;

namespace Domain.Model.Entities;

/// <summary>
/// Clase que representa un crédito
/// </summary>
public class Credito
{
    /// <summary>
    /// Inicializa la fecha de solicitud a la fecha actual
    /// </summary>
    public Credito()
    {
        FechaDeSolicitud = DateTime.Now;
        Pagos = new List<Pago>();
    }

    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Monto del crédito solicitado
    /// </summary>
    public decimal Monto { get; set; }

    /// <summary>
    /// Numero de cuotas en las que se pagará el crédito
    /// </summary>
    public int PlazoEnMeses { get; set; }

    /// <summary>
    /// Numero de cuotas pagadas hasta el momento
    /// </summary>
    public int CuotasPagadas { get; set; }

    /// <summary>
    /// Tasa de interés del crédito
    /// </summary>
    public decimal TasaDeInteres { get; set; }

    /// <summary>
    /// El valor de cada cuota
    /// </summary>
    public decimal MontoPorCuota { get; set; }

    /// <summary>
    /// Total de intereses pagados
    /// </summary>
    public decimal TotalIntereses { get; set; }

    /// <summary>
    /// Cuotas que faltan por pagar
    /// </summary>
    public int CuotasRestantes { get; set; }

    /// <summary>
    /// Fecha de solicitud del crédito
    /// </summary>
    public DateTime FechaDeSolicitud { get; set; }

    /// <summary>
    /// lista de pagos realizados
    /// </summary>
    public List<Pago> Pagos { get; set; }

    /// <summary>
    /// Método para pagar cuota del crédito
    /// </summary>
    /// <param name="pago"></param>
    public void PagarCuota(Pago pago)
    {
        ValidarPagoMinimo(pago);
        Pagos.Add(pago);
        CuotasRestantes -= 1;
        CuotasPagadas += 1;
    }

    /// <summary>
    /// Método para validar que el pago corresponda al valor de la cuota
    /// </summary>
    /// <param name="pago"></param>
    /// <exception cref="BusinessException"></exception>
    private void ValidarPagoMinimo(Pago pago)
    {
        if (pago.Monto < MontoPorCuota)
        {
            throw new BusinessException($"El valor de la cuota es {MontoPorCuota}",
                (int)TipoExcepcionNegocio.ValorDeCuotaInvalido);
        }
    }

    /// <summary>
    /// Método para calcular el interés del crédito según la tasa de interés efectiva anual
    /// </summary>
    /// <param name="tasaInteresEA"></param>
    public void CalcularTasaInteres(decimal tasaInteresEA)
    {
        TasaDeInteres = (decimal)(Math.Pow((double)(1 + tasaInteresEA), (1 / 12.0)) - 1);
    }

    /// <summary>
    /// Método para calcular el valor de la cuota
    /// </summary>
    public void CalcularCuota()
    {
        var calculo = (decimal)Math.Pow((double)(1 + TasaDeInteres), PlazoEnMeses);
        MontoPorCuota = Math.Round(((TasaDeInteres * calculo) * Monto) / (calculo - 1), 2);
    }

    /// <summary>
    /// Método para calcular el total de intereses pagados
    /// </summary>
    public void CalcularTotalIntereses()
    {
        TotalIntereses = Math.Round((MontoPorCuota * PlazoEnMeses) - Monto, 2);
    }
}