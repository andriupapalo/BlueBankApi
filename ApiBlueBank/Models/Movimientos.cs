using System;
using System.Collections.Generic;

namespace ApiBlueBank.Models
{
    public partial class Movimientos
    {
        public int Id { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public int? TipoCuentaId { get; set; }
        public string CuentaId { get; set; }
        public int? MovimientoId { get; set; }
        public bool? TipoAfectacionSuma { get; set; }
        public double SaldoAnterior { get; set; }
        public double ValorMovimiento { get; set; }
        public double NuevoSaldo { get; set; }

        public virtual TipoMovimiento Movimiento { get; set; }
        public virtual TipoCuenta TipoCuenta { get; set; }
        public virtual Cuentas Cuenta { get; set; }
    }
}
