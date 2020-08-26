using System;
using System.Collections.Generic;

namespace ApiBlueBank.Models
{
    public partial class TipoMovimiento
    {
        public TipoMovimiento()
        {
            Movimientos = new HashSet<Movimientos>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool? TipoAfectacionSuma { get; set; }

        public virtual ICollection<Movimientos> Movimientos { get; set; }
    }
}
