using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioItix.Data;
using DesafioItix.Models;

namespace DesafioItix.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly DesafioContext _context;
        private readonly IDataRepository<Consulta> _repo;

        public ConsultasController(DesafioContext context, IDataRepository<Consulta> repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<Consulta> GetConsultas()
        {
            return _context.Consulta.OrderByDescending(o => o.Id);
        }

        [HttpPost("FilteredConsultas")]
        public IEnumerable<Consulta> FilteredConsultas([FromBody] ConsultaByFilteredSearch consulta)
        {
            List<Consulta> filteredConsultas = _context.Consulta.OrderByDescending(o => o.Id).ToList();
            if (consulta.Id != null)
            {
                filteredConsultas = filteredConsultas.Where(c => c.Id == consulta.Id).ToList();
            }
            if (consulta.DataInicial != null)
            {
                filteredConsultas = filteredConsultas.Where(c => c.DataInicial == consulta.DataInicial).ToList();
            }
            if (consulta.DataFinal != null)
            {
                filteredConsultas = filteredConsultas.Where(c => c.DataFinal == consulta.DataFinal).ToList();
            }
            if (consulta.DataNascimento != null)
            {
                filteredConsultas = filteredConsultas.Where(c => c.DataNascimento == consulta.DataNascimento).ToList();
            }
            if (!String.IsNullOrEmpty(consulta.Nome))
            {
                filteredConsultas = filteredConsultas.Where(c => c.Nome.Contains(consulta.Nome)).ToList();
            }
            if (consulta.Observacoes != null)
            {
                filteredConsultas = filteredConsultas.Where(c => c.Observacoes.Contains(consulta.Observacoes)).ToList();
            }
            return filteredConsultas;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsulta([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var consulta = await _context.Consulta.FindAsync(id);

            if (consulta == null)
            {
                return NotFound();
            }

            return Ok(consulta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsulta([FromRoute] int id, [FromBody] Consulta consulta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != consulta.Id)
            {
                return BadRequest();
            }

            _context.Entry(consulta).State = EntityState.Modified;

            try
            {
                _repo.Update(consulta);
                var save = await _repo.SaveAsync(consulta);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultaExists(id))
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
        public async Task<IActionResult> PostConsulta([FromBody] Consulta consulta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.Add(consulta);
            Consulta savedConsulta = await _repo.SaveAsync(consulta);

            return Ok(savedConsulta);
        }

        [HttpPost("PostValidateConsulta")]
        public bool PostValidateConsulta([FromBody] Consulta consulta)
        {
            bool validConsulta;
            List<Consulta> consultasComDataImpedimento =
                _context.Consulta
                .Where(c =>
                (consulta.DataInicial >= c.DataInicial && consulta.DataInicial <= c.DataFinal) ||
                (consulta.DataFinal >= c.DataInicial && consulta.DataFinal <= c.DataFinal))
                .ToList();

            if (consultasComDataImpedimento.Count() > 0)
            {
                validConsulta = consultasComDataImpedimento.Where(c => c.Id != consulta.Id).Count() == 0;
            }
            else
            {
                validConsulta = true;
            }

            return validConsulta;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsulta([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            _repo.Delete(consulta);
            var save = await _repo.SaveAsync(consulta);

            return Ok(consulta);
        }

        private bool ConsultaExists(int id)
        {
            return _context.Consulta.Any(e => e.Id == id);
        }
    }
}
