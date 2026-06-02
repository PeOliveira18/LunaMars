using LunaMars.API.Data;
using LunaMars.API.Exceptions;
using LunaMars.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunaMars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColoniasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ColoniasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColoniaEspacial>>> Get()
        {
            return await _context.Colonias
                .Include(c => c.Setores)
                .Include(c => c.Recursos)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ColoniaEspacial>> Get(int id)
        {
            var colonia = await _context.Colonias
                .Include(c => c.Setores)
                .Include(c => c.Recursos)
                .FirstOrDefaultAsync(c => c.idColonia == id);

            if (colonia == null)
            {
                return NotFound("Colonia nao encontrada.");
            }

            return colonia;
        }

        [HttpPost]
        public async Task<ActionResult<ColoniaEspacial>> Post(ColoniaEspacial colonia)
        {
            try
            {
                ValidarColonia(colonia);

                colonia.dtCriacao = DateTime.Now;

                _context.Colonias.Add(colonia);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = colonia.idColonia }, colonia);
            }
            catch (ColoniaInvalidaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var colonia = await _context.Colonias.FindAsync(id);

            if (colonia == null)
            {
                return NotFound("Colonia nao encontrada.");
            }

            var setores = await _context.Setores
                .Where(s => s.coloniaEspacialId == id)
                .ToListAsync();
            var setorIds = setores.Select(s => s.idSetor).ToList();

            if (setorIds.Any())
            {
                var leituras = await _context.Leituras
                    .Where(l => setorIds.Contains(l.setorColoniaId))
                    .ToListAsync();
                var sensores = await _context.Sensores
                    .Where(s => setorIds.Contains(s.setorColoniaId))
                    .ToListAsync();
                var alertas = await _context.Alertas
                    .Where(a => setorIds.Contains(a.setorColoniaId))
                    .ToListAsync();
                var alertaIds = alertas.Select(a => a.idAlerta).ToList();

                if (alertaIds.Any())
                {
                    var missoes = await _context.Missoes
                        .Where(m => alertaIds.Contains(m.alertaColoniaId))
                        .ToListAsync();

                    _context.Missoes.RemoveRange(missoes);
                }

                _context.Leituras.RemoveRange(leituras);
                _context.Sensores.RemoveRange(sensores);
                _context.Alertas.RemoveRange(alertas);
                _context.Setores.RemoveRange(setores);
            }

            var recursos = await _context.Recursos
                .Where(r => r.coloniaEspacialId == id)
                .ToListAsync();

            _context.Recursos.RemoveRange(recursos);
            _context.Colonias.Remove(colonia);

            await _context.SaveChangesAsync();

            return Ok("Colonia removida com sucesso.");
        }

        private void ValidarColonia(ColoniaEspacial colonia)
        {
            if (string.IsNullOrWhiteSpace(colonia.nomeColonia))
            {
                throw new ColoniaInvalidaException("O nome da colonia e obrigatorio.");
            }

            if (colonia.capacidadePessoas <= 0)
            {
                throw new ColoniaInvalidaException("A capacidade de pessoas deve ser maior que zero.");
            }
        }
    }
}
