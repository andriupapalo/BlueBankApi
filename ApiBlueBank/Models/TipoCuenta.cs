using System;
using System.Collections.Generic;

namespace ApiBlueBank.Models
{
    public partial class TipoCuenta
    {
        public TipoCuenta()
        {
            Cuentas = new HashSet<Cuentas>();
            Movimientos = new HashSet<Movimientos>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Cuentas> Cuentas { get; set; }
        public virtual ICollection<Movimientos> Movimientos { get; set; }
    }
}
