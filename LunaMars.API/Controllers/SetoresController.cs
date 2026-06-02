using LunaMars.API.Data;
using LunaMars.API.Exceptions;
using LunaMars.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunaMars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SetoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SetorColonia>>> Get()
        {
            return await _context.Setores
                .Include(s => s.ColoniaEspacial)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SetorColonia>> Get(int id)
        {
            var setor = await _context.Setores.FindAsync(id);

            if (setor == null)
            {
                return NotFound("Setor nao encontrado.");
            }

            return setor;
        }

        [HttpGet("colonia/{coloniaId:int}")]
        public async Task<ActionResult<IEnumerable<SetorColonia>>> GetPorColonia(int coloniaId)
        {
            return await _context.Setores
                .Where(s => s.coloniaEspacialId == coloniaId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<SetorColonia>> Post(SetorColonia setor)
        {
            try
            {
                ValidarSetor(setor);

                var colonia = await _context.Colonias.FindAsync(setor.coloniaEspacialId);

                if (colonia == null)
                {
                    return NotFound("Colonia nao encontrada.");
                }

                setor.ativo = true;

                _context.Setores.Add(setor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = setor.idSetor }, setor);
            }
            catch (SetorInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var setor = await _context.Setores.FindAsync(id);

            if (setor == null)
            {
                return NotFound("Setor nao encontrado.");
            }

            var leituras = await _context.Leituras
                .Where(l => l.setorColoniaId == id)
                .ToListAsync();
            var sensores = await _context.Sensores
                .Where(s => s.setorColoniaId == id)
                .ToListAsync();
            var alertas = await _context.Alertas
                .Where(a => a.setorColoniaId == id)
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
            _context.Setores.Remove(setor);

            await _context.SaveChangesAsync();

            return Ok("Setor removido com sucesso.");
        }

        private void ValidarSetor(SetorColonia setor)
        {
            if (string.IsNullOrWhiteSpace(setor.nomeSetor))
            {
                throw new SetorInvalidoException("O nome do setor e obrigatorio.");
            }

            if (setor.pressaoInterna < 0)
            {
                throw new SetorInvalidoException("A pressao interna nao pode ser negativa.");
            }

            if (setor.temperaturaAtual < -150 || setor.temperaturaAtual > 80)
            {
                throw new SetorInvalidoException("A temperatura do setor esta fora do intervalo permitido.");
            }
        }
    }
}
