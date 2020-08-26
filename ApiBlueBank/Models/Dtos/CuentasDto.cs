using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiBlueBank.Models.Dtos
{
    public class CuentasDto
    {
        public string Id { get; set; }
        public string ClienteId { get; set; }
        public int? TipoCuentaId { get; set; }
        public int? SucursalId { get; set; }
        public DateTime FechaApertura { get; set; }
        public double? MontoInicial { get; set; }
        public double? SaldoActual { get; set; }
        public bool? Activo { get; set; }
    }
}
