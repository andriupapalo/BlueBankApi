using System;
using System.Collections.Generic;

namespace ApiBlueBank.Models
{
    public partial class Sucursal
    {
        public Sucursal()
        {
            Cuentas = new HashSet<Cuentas>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Cuentas> Cuentas { get; set; }
    }
}
