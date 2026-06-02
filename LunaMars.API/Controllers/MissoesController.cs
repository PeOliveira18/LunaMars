using LunaMars.API.Data;
using LunaMars.API.Enums;
using LunaMars.API.Exceptions;
using LunaMars.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunaMars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MissoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MissaoResposta>>> Get()
        {
            return await _context.Missoes
                .Include(m => m.AlertaColonia)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MissaoResposta>> Get(int id)
        {
            var missao = await _context.Missoes.FindAsync(id);

            if (missao == null)
            {
                return NotFound("Missao nao encontrada.");
            }

            return missao;
        }

        [HttpPost]
        public async Task<ActionResult<MissaoResposta>> Post(MissaoResposta missao)
        {
            try
            {
                ValidarMissao(missao);

                var alerta = await _context.Alertas.FindAsync(missao.alertaColoniaId);

                if (alerta == null)
                {
                    throw new AlertaNaoEncontradoException("Alerta nao encontrado.");
                }

                if (alerta.statusAlerta == StatusAlerta.Finalizado)
                {
                    throw new MissaoInvalidaException("Nao e possivel criar missao para um alerta finalizado.");
                }

                missao.dtInicio = DateTime.Now;
                missao.statusMissao = StatusMissao.EmAndamento;

                _context.Missoes.Add(missao);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = missao.idMissao }, missao);
            }
            catch (AlertaNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (MissaoInvalidaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}/concluir")]
        public async Task<ActionResult<MissaoResposta>> Concluir(int id)
        {
            var missao = await _context.Missoes.FindAsync(id);

            if (missao == null)
            {
                return NotFound("Missao nao encontrada.");
            }

            missao.statusMissao = StatusMissao.Concluida;
            missao.dtFim = DateTime.Now;

            await _context.SaveChangesAsync();

            return missao;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var missao = await _context.Missoes.FindAsync(id);

            if (missao == null)
            {
                return NotFound("Missao nao encontrada.");
            }

            _context.Missoes.Remove(missao);
            await _context.SaveChangesAsync();

            return Ok("Missao removida com sucesso.");
        }

        private void ValidarMissao(MissaoResposta missao)
        {
            if (string.IsNullOrWhiteSpace(missao.descricao))
            {
                throw new MissaoInvalidaException("A descricao da missao e obrigatoria.");
            }
        }
    }
}
