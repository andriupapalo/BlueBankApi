using System;
using System.Collections.Generic;

namespace ApiBlueBank.Models
{
    public partial class Clientes
    {
        public Clientes()
        {
            Cuentas = new HashSet<Cuentas>();
        }

        public string Id { get; set; }
        public int DocumentoId { get; set; }
        public string NombreCompleto { get; set; }
        public string DireccionResidencia { get; set; }
        public string TelefonoContacto { get; set; }
        public string Email { get; set; }
        public string ClaveWeb { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }

        public virtual TipoDocumento Documento { get; set; }
        public virtual ICollection<Cuentas> Cuentas { get; set; }
    }
}
