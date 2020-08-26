using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiBlueBank.Models.Dtos
{
    public class MovimientosDto
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
    }
}
