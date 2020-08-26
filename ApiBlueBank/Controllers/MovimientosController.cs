using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBlueBank.Models;
using Microsoft.AspNetCore.Cors;
using FluentValidation;
using ApiBlueBank.Models.Dtos;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiBlueBank.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly BANCOContext _context;
        private readonly IValidator<MovimientosDto> _valMvto;

        public MovimientosController(BANCOContext context, IValidator<MovimientosDto> valmvto)
        {
            _context = context;
            _valMvto = valmvto;
        }


        [HttpGet("TipoMovimiento")]
        [EnableCors("MyPolicy")]
        public async Task<ActionResult<IEnumerable<TipoMovimiento>>> TipoMovimiento()
        {
            var tipomovimiento = await _context.TipoMovimiento.ToListAsync();
            var tipomovimientodto = tipomovimiento.Select(x => new TipoMovimientoDto
            {
                Id = x.Id,
                Descripcion = x.Descripcion.Trim(),
                TipoAfectacionSuma=x.TipoAfectacionSuma
            });
            if (tipomovimiento != null)
            {
                return Ok(tipomovimientodto);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movimientos>>> GetMovimientos()
        {
            var movimiento= await _context.Movimientos.ToListAsync();
            var movimientodto = movimiento.Select(x => new MovimientosDto { 
                    Id= x.Id,
                    FechaMovimiento = x.FechaMovimiento,
                    TipoCuentaId=x.TipoCuentaId,
                    CuentaId=x.CuentaId.Trim(),
                    MovimientoId=x.MovimientoId,
                    TipoAfectacionSuma=x.TipoAfectacionSuma,
                    SaldoAnterior=x.SaldoAnterior,
                    ValorMovimiento=x.ValorMovimiento,
                    NuevoSaldo=x.NuevoSaldo
            });
            if (movimiento!=null)
                {
                return Ok(movimientodto);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("Detalle")]
        [EnableCors("MyPolicy")]
        public async Task<ActionResult<IEnumerable<Movimientos>>> GetMovimientosDetalle(string nocuenta,DateTime fecha1, DateTime fecha2)
        {
            var movimiento = await _context.Movimientos.Where(x=>x.CuentaId== nocuenta && (x.FechaMovimiento>= fecha1 && x.FechaMovimiento <= fecha2)).ToListAsync();
            var movimientodto = movimiento.Select(x => new MovimientosDto
            {
                Id = x.Id,
                FechaMovimiento = x.FechaMovimiento,
                TipoCuentaId = x.TipoCuentaId,
                CuentaId = x.CuentaId.Trim(),
                MovimientoId = x.MovimientoId,
                TipoAfectacionSuma = x.TipoAfectacionSuma,
                SaldoAnterior = x.SaldoAnterior,
                ValorMovimiento = x.ValorMovimiento,
                NuevoSaldo = x.NuevoSaldo
            });
            if (movimiento != null)
            {
                return Ok(movimientodto);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Movimientos>> GetMovimientosById(int id)
        {
            var movimientos = await _context.Movimientos.FindAsync(id);
            var movimientodto = new MovimientosDto
            {
                Id = movimientos.Id,
                FechaMovimiento = movimientos.FechaMovimiento,
                TipoCuentaId = movimientos.TipoCuentaId,
                CuentaId = movimientos.CuentaId.Trim(),
                MovimientoId = movimientos.MovimientoId,
                TipoAfectacionSuma = movimientos.TipoAfectacionSuma,
                SaldoAnterior = movimientos.SaldoAnterior,
                ValorMovimiento = movimientos.ValorMovimiento,
                NuevoSaldo = movimientos.NuevoSaldo
            };

            if (movimientos == null)
            {
                return NotFound();
            }

            return Ok(movimientodto);
        }

       /* [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimientos(int id, Movimientos movimientos)
        {
            if (id != movimientos.Id)
            {
                return BadRequest();
            }

            _context.Entry(movimientos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovimientosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        [HttpPost]
        public async Task<ActionResult<Movimientos>> PostMovimientos(MovimientosDto movimientosdto)
        {
            List<String> resultado = new List<String>();
            var validamovimiento = _valMvto.Validate(movimientosdto);
            if (!validamovimiento.IsValid)
            {
                foreach(var error in validamovimiento.Errors)
                {
                    resultado.Add(error.ToString());
                }
                return new BadRequestObjectResult(resultado);
            }

            var movimientos = new Movimientos
            {
                FechaMovimiento = DateTime.Now,
                TipoCuentaId = movimientosdto.TipoCuentaId,
                CuentaId = movimientosdto.CuentaId.Trim(),
                MovimientoId = movimientosdto.MovimientoId,
                TipoAfectacionSuma = movimientosdto.TipoAfectacionSuma,
                SaldoAnterior = movimientosdto.SaldoAnterior,
                ValorMovimiento = movimientosdto.ValorMovimiento,
                NuevoSaldo= (movimientosdto.TipoAfectacionSuma==true ? (movimientosdto.SaldoAnterior + movimientosdto.ValorMovimiento) : (movimientosdto.SaldoAnterior - movimientosdto.ValorMovimiento))
            };
            _context.Movimientos.Add(movimientos);
            await _context.SaveChangesAsync();

            //aqui se actualiza el saldo de la cuenta
            SqlConnection con = (SqlConnection)_context.Database.GetDbConnection();
            SqlCommand cmd = con.CreateCommand();
            con.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_ActualizaSaldo";
            cmd.Parameters.Add("@Ncuenta", System.Data.SqlDbType.VarChar, 20).Value = movimientosdto.CuentaId.Trim();
            cmd.Parameters.Add("@Nvalor", System.Data.SqlDbType.Int).Value = movimientos.NuevoSaldo;
            cmd.ExecuteNonQuery();
            con.Close();
            return CreatedAtAction("GetMovimientos", new { id = movimientos.Id }, movimientosdto);
        }

        // DELETE: api/Movimientos/5
        /*[HttpDelete("{id}")]
        public async Task<ActionResult<Movimientos>> DeleteMovimientos(int id)
        {
            var movimientos = await _context.Movimientos.FindAsync(id);
            if (movimientos == null)
            {
                return NotFound();
            }

            _context.Movimientos.Remove(movimientos);
            await _context.SaveChangesAsync();

            return movimientos;
        }*/

        private bool MovimientosExists(int id)
        {
            return _context.Movimientos.Any(e => e.Id == id);
        }
    }
}
