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

namespace ApiBlueBank.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly BANCOContext _context;
        private readonly IValidator<CuentasDto> _valcuenta;
        public CuentasController(BANCOContext context, IValidator<CuentasDto> valcuenta)
        {
            _context = context;
            _valcuenta = valcuenta;
        }

        [HttpGet("Sucursal")]
        public async Task<ActionResult<IEnumerable<Sucursal>>> GetSucursal()
        {
            var sucursal = await _context.Sucursal.ToListAsync();
            var sucursaldto = sucursal.Select(x => new SucursalDto
            {
                Id = x.Id,
                Descripcion = x.Descripcion.Trim()
            });
            if (sucursal!=null)
            {
                return Ok(sucursaldto);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("TipoCuenta")]
        public async Task<ActionResult<IEnumerable<TipoCuenta>>> TipoCuenta()
        {
            var tipocuenta = await _context.TipoCuenta.ToListAsync();
            var tipocuentadto = tipocuenta.Select(x => new TipoCuentaDto
            {
                Id = x.Id,
                Descripcion = x.Descripcion.Trim()
            });
            if (tipocuenta != null)
            {
                return Ok(tipocuentadto);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuentas>>> GetCuentas()
        {

            var cuenta= await _context.Cuentas.ToListAsync();
            var cuentadto = cuenta.Select(x => new CuentasDto
            {
                Id = x.Id.Trim(),
                ClienteId = x.ClienteId,
                TipoCuentaId = x.TipoCuentaId,
                SucursalId = x.SucursalId,
                FechaApertura=x.FechaApertura,
                MontoInicial = x.MontoInicial,
                SaldoActual = x.SaldoActual,
                Activo = x.Activo
            });
            return Ok(cuentadto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Cuentas>> GetCuentas(string id)
        {
            var cuentas = await _context.Cuentas.FindAsync(id);

            if (cuentas == null)
            {
                return NotFound();
            }
            var cuentadto = new CuentasDto
            {
                Id = cuentas.Id.Trim(),
                ClienteId = cuentas.ClienteId,
                TipoCuentaId = cuentas.TipoCuentaId,
                SucursalId = cuentas.SucursalId,
                FechaApertura=cuentas.FechaApertura,
                MontoInicial = cuentas.MontoInicial,
                SaldoActual = cuentas.SaldoActual,
                Activo=cuentas.Activo
            };
            return Ok(cuentadto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuentas(string id, CuentasDto cuentasdto)
        {
            // validamos que el registro exista
            var cuentasres = await _context.Cuentas.FindAsync(id);

            if (cuentasres == null)
            {
                return NotFound();
            }


            var cuentas = new Cuentas
            {
                Id = cuentasdto.Id.Trim(),
                ClienteId = cuentasdto.ClienteId,
                TipoCuentaId = cuentasdto.TipoCuentaId,
                SucursalId = cuentasdto.SucursalId,
                FechaApertura= cuentasres.FechaApertura,
                MontoInicial = cuentasdto.MontoInicial,
                SaldoActual = cuentasdto.SaldoActual,
                Activo = cuentasdto.Activo
            };

            if (id != cuentas.Id)
            {
                return BadRequest();
            }

            _context.Entry(cuentas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<Cuentas>> PostCuentas(CuentasDto cuentasdto)
        {
           List<String> respuesta = new List<String>();
            var validacioncuentas = _valcuenta.Validate(cuentasdto);
            if (!validacioncuentas.IsValid)
            {
                foreach(var error in validacioncuentas.Errors)
                {
                    respuesta.Add(error.ToString());
                }
                return new BadRequestObjectResult(respuesta);
            }
            var cuentas = new Cuentas
            {
                Id = cuentasdto.Id.Trim(),
                ClienteId = cuentasdto.ClienteId,
                TipoCuentaId = cuentasdto.TipoCuentaId,
                SucursalId = cuentasdto.SucursalId,
                FechaApertura = DateTime.Now,
                MontoInicial = cuentasdto.MontoInicial,
                SaldoActual = cuentasdto.SaldoActual,
                Activo=cuentasdto.Activo
            };
            _context.Cuentas.Add(cuentas);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CuentasExists(cuentas.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCuentas", new { id = cuentas.Id }, cuentasdto);
        }

        /*[HttpDelete("{id}")]
        public async Task<ActionResult<Cuentas>> DeleteCuentas(string id)
        {
            var cuentas = await _context.Cuentas.FindAsync(id);
            if (cuentas == null)
            {
                return NotFound();
            }

            _context.Cuentas.Remove(cuentas);
            await _context.SaveChangesAsync();

            return cuentas;
        }*/

        private bool CuentasExists(string id)
        {
            return _context.Cuentas.Any(e => e.Id == id);
        }
    }
}
