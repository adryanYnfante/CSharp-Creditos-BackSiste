using FluentValidation;

namespace EntryPoints.ReactiveWeb.Dtos.Validations;

public class CrearCreditoValidation : AbstractValidator<CrearCreditoRequest>
{
    public CrearCreditoValidation()
    {
        RuleFor(c => c.Monto).NotEmpty().WithMessage("Digite un valor");
        RuleFor(c => c.PlazoEnMeses).NotEmpty().Must(v => v > 1).WithMessage("El plazo debe ser de almenos un mes");
        RuleFor(c => c.numeroDeDocumento).NotEmpty().MinimumLength(4).WithMessage("La cedula debe contener almenos 4 digitos");
    }
}