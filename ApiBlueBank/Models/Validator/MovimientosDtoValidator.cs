using ApiBlueBank.Models.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiBlueBank.Models.Validator
{
    public class MovimientosDtoValidator:AbstractValidator<MovimientosDto>
    {
        public MovimientosDtoValidator()
        {
            RuleFor(x => x.TipoCuentaId).GreaterThan(0)
                .WithMessage("Digite el Tipo de cuenta");
            RuleFor(x => x.CuentaId).NotEmpty().Length(5, 20)
                .WithMessage("Numero de cuenta erroneo");
            RuleFor(x => x.MovimientoId).GreaterThan(0)
                .WithMessage("Digite el Tipo de cuenta");
            RuleFor(x => x.ValorMovimiento).GreaterThan(0)
                .WithMessage("Digite el Tipo de cuenta");
        }
    }
}
