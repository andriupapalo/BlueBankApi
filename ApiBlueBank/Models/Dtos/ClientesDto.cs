using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiBlueBank.Models.Dtos
{
    public class ClientesDto
    {
        public string Id { get; set; }
        public int DocumentoId {get;set;}
        public string NombreCompleto {get;set;}
        public string DireccionResidencia {get;set;}
        public string TelefonoContacto {get;set;}
        public string Email {get;set;}
        public string ClaveWeb {get;set;}
        public DateTime FechaNacimiento {get;set;}
        public int Edad {get;set;}
        public string Sexo {get;set;}

    }
    //        public DateTime FechaNacimiento { get; set; }
}


