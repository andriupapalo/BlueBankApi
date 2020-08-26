using ApiBlueBank.Models;
using ApiBlueBank.Models.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiBlueBank.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly BANCOContext _context;
        private readonly IValidator<ClientesDto> _valcliente;

        public ClientesController(BANCOContext context, IValidator<ClientesDto> valcliente)
        {
            _valcliente = valcliente;
            _context = context;

        }

        [HttpGet("TipoDocumento")]
        public async Task<ActionResult<IEnumerable<TipoDocumento>>> TipoDocumento()
        {
            var tipodocumento = await _context.TipoDocumento.ToListAsync();
            var tipodocumentodto = tipodocumento.Select(x => new TipoDocumentoDto
            {
                Id = x.Id,
                Descripcion = x.Descripcion.Trim()
            });
            if (tipodocumento != null)
            {
                return Ok(tipodocumentodto);
            }
            else
            {
                return NotFound();
            }

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clientes>>> GetClientes()
        {
            var cliente = await _context.Clientes.ToListAsync();
            var clientedto = cliente.Select(x => new ClientesDto
            {
                Id = x.Id.Trim(),
                DocumentoId = x.DocumentoId,
                NombreCompleto = x.NombreCompleto.Trim(),
                DireccionResidencia = x.DireccionResidencia.Trim(),
                TelefonoContacto = x.TelefonoContacto.Trim(),
                Email = x.Email.Trim(),
                ClaveWeb = x.ClaveWeb.Trim(),
                FechaNacimiento = x.FechaNacimiento,
                Edad = x.Edad,
                Sexo = x.Sexo
            });


            return Ok(clientedto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Clientes>> GetClientesById(string id)
        {
            var clientes = await _context.Clientes.FindAsync(id);
            if (clientes == null)
            {
                return NotFound();
            }
            var clientedto = new ClientesDto
            {
                Id = clientes.Id.Trim(),
                DocumentoId = clientes.DocumentoId,
                NombreCompleto = clientes.NombreCompleto.Trim(),
                DireccionResidencia = clientes.DireccionResidencia.Trim(),
                TelefonoContacto = clientes.TelefonoContacto.Trim(),
                Email = clientes.Email.Trim(),
                ClaveWeb = clientes.ClaveWeb.Trim(),
                FechaNacimiento = clientes.FechaNacimiento,
                Edad = clientes.Edad,
                Sexo = clientes.Sexo
            };

            return Ok(clientedto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientes(string id, ClientesDto clientesDto)
        {
            var clientes = new Clientes
            {
                Id = clientesDto.Id.Trim(),
                DocumentoId = clientesDto.DocumentoId,
                NombreCompleto = clientesDto.NombreCompleto.Trim(),
                DireccionResidencia = clientesDto.DireccionResidencia.Trim(),
                TelefonoContacto = clientesDto.TelefonoContacto.Trim(),
                Email = clientesDto.Email.Trim(),
                ClaveWeb = clientesDto.ClaveWeb.Trim(),
                FechaNacimiento = clientesDto.FechaNacimiento,
                Edad = clientesDto.Edad,
                Sexo = clientesDto.Sexo
            };


            if (id != clientes.Id)
            {
                return BadRequest();
            }
            _context.Entry(clientes).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    //throw;
                    return BadRequest();
                }
            }
            return NoContent();
        }






        [HttpPost]
        public async Task<ActionResult<Clientes>> PostClientes(ClientesDto clientesDto)
        //public async Task<ActionResult<Clientes>> PostClientes(ClientesDto clientesDto)
        {
            List<String> respuesta = new List<String>();
            var resuladovalidacion = _valcliente.Validate(clientesDto);
            if (!resuladovalidacion.IsValid)
            {
                foreach (var error in resuladovalidacion.Errors)
                {
                    respuesta.Add(error.ToString());
                }
                return new BadRequestObjectResult(respuesta);
            }

            var clientes = new Clientes
            {
                Id = clientesDto.Id.Trim(),
                DocumentoId = clientesDto.DocumentoId,
                NombreCompleto = clientesDto.NombreCompleto.Trim(),
                DireccionResidencia = clientesDto.DireccionResidencia.Trim(),
                TelefonoContacto = clientesDto.TelefonoContacto.Trim(),
                Email = clientesDto.Email.Trim(),
                ClaveWeb = clientesDto.ClaveWeb.Trim(),
                FechaNacimiento = clientesDto.FechaNacimiento,
                Edad = clientesDto.Edad,
                Sexo = clientesDto.Sexo
            };


            _context.Clientes.Add(clientes);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientesExists(clientes.Id))
                {
                    return Conflict();
                }
                else
                {
                    //throw;
                    return BadRequest();
                }
            }

            return CreatedAtAction("GetClientes", new { id = clientes.Id }, clientesDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Clientes>> DeleteClientes(string id)
        {
            var clientes = await _context.Clientes.FindAsync(id);
            if (clientes == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(clientes);
            await _context.SaveChangesAsync();

            return clientes;
        }

        private bool ClientesExists(string id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
