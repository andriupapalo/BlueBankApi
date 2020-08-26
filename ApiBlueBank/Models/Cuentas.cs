using System;
using System.Collections.Generic;

namespace ApiBlueBank.Models
{
    public partial class Cuentas
    {
        public string Id { get; set; }
        public string ClienteId { get; set; }
        public int? TipoCuentaId { get; set; }
        public int? SucursalId { get; set; }
        public DateTime FechaApertura { get; set; }
        public double? MontoInicial { get; set; }
        public double? SaldoActual { get; set; }
        public bool? Activo { get; set; }

        public virtual Clientes Cliente { get; set; }
        public virtual TipoCuenta Cuenta { get; set; }
        public virtual Sucursal Sucursal { get; set; }
    }
}
