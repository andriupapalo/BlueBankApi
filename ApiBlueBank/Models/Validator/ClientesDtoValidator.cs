using ApiBlueBank.Models.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ApiBlueBank.Models.Validator
{
    public class ClientesDtoValidator:AbstractValidator<ClientesDto>
    {
        public ClientesDtoValidator()
        {
                        RuleFor(x => x.Id).NotEmpty().Length(5, 20)
                        .WithMessage("Por favor Digite El numero de documento Minimo 5 Maximo 20");
                        RuleFor(x => x.DocumentoId).NotEmpty().GreaterThan(0)
                            .WithMessage("Por favor seleccione tipo de Documento");
                        RuleFor(x=> x.NombreCompleto).NotEmpty().MinimumLength(5)
                            .WithMessage("Por favor Digite un nombre Valido");
                        RuleFor(x => x.DireccionResidencia).NotEmpty().Length(5, 100)
                           .WithMessage("Por favor digite direccion de la residencia");
                        RuleFor(x => x.TelefonoContacto).NotEmpty().Length(5, 20)
                            .WithMessage("Por favor diligencie el numero de telefono");
                        RuleFor(x => x.Email).NotEmpty().EmailAddress()
                            .WithMessage("Por favor digite un mail valido");
                        RuleFor(x => x.ClaveWeb).NotEmpty().MinimumLength(3)
                            .WithMessage("Favor Digite la clave Web");
                        RuleFor(x => x.FechaNacimiento).Must(validafecha)
                            .WithMessage("Fecha de nacimiento Invalida");
                        RuleFor(x => x.Edad).NotEmpty().GreaterThan(0)
                            .WithMessage("Por favro especifique La edad");
                        RuleFor(x => x.Sexo).NotEmpty().MaximumLength(1).Must(Validasexo)
                            .WithMessage("Por favor especifique el sexo F o M unicamente");
        }
        private bool Validasexo(String Sexo)
        {
            return (Sexo == "F" || Sexo == "M");
        }
        private bool validafecha(DateTime FechaNacimiento)
        {
            return (FechaNacimiento <= DateTime.Today.AddYears(-18));
        }

    }
}

