using FluentValidation;
using System.Text.RegularExpressions;

namespace EntryPoints.ReactiveWeb.Dtos.Validations;

public class ModificarClienteValidation : AbstractValidator<CrearClienteDto>
{
    public ModificarClienteValidation()
    {
        RuleFor(c => c.Correo).Must(e => Regex.IsMatch(e,
                @"\A(?:[a-z0-9!#$%&'+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&'+/=?^_`{|}~-]+)@(?:[a-z0-9](?:[a-z0-9-][a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase)).WithMessage("Email inválido");
        RuleFor(c => c.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacío");
        RuleFor(c => c.Apellidos).NotEmpty().WithMessage("Los apellidos no pueden estar vacios");
        RuleFor(c => c.NumeroDeTelefono).NotEmpty().MinimumLength(10).MaximumLength(10).WithMessage("El número de celular debe contener 10 digitos");
    }
}