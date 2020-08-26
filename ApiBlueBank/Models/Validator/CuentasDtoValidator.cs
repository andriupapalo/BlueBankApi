using ApiBlueBank.Models.Dtos;
using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ApiBlueBank.Models.Validator
{
    public class CuentasDtoValidator:AbstractValidator<CuentasDto>
    {
        public CuentasDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().Length(5, 20)
                .WithMessage("Por favor diligencie el numero de Cuenta");

            RuleFor(x => x.ClienteId).NotEmpty().Length(5, 20)
                .WithMessage("Por favor diligencie el numero de identificacion");
            
            RuleFor(x => x.TipoCuentaId).GreaterThan(0)
                .WithMessage("Por favor diligencie el Tipo de Cuenta");
            
            RuleFor(x => x.SucursalId).GreaterThan(0)
                .WithMessage("Por favor diligencie la sucursal de Afiliacion");

            RuleFor(x => x.SucursalId).GreaterThan(0)
                .WithMessage("Por favor diligencie la sucursal de Afiliacion");

            RuleFor(x => x.MontoInicial).GreaterThan(0)
                .WithMessage("Por favor diligencie Monto Inicial de Afiliacion");

            RuleFor(x => x.SaldoActual).GreaterThan(0)
                .WithMessage("Por favor diligencie Saldo Actual de Afiliacion");

        }
    }
}

