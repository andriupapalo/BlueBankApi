using System;
using System.Collections.Generic;

namespace ApiBlueBank.Models
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Clientes = new HashSet<Clientes>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Clientes> Clientes { get; set; }
    }
}
