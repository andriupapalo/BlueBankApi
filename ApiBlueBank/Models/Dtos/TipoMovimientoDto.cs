using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiBlueBank.Models.Dtos
{
    public class TipoMovimientoDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool? TipoAfectacionSuma { get; set; }
    }
}
