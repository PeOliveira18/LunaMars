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
    public class AlertasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AlertasController> _logger;

        public AlertasController(AppDbContext context, ILogger<AlertasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlertaColonia>>> Get()
        {
            return await _context.Alertas
                .Include(a => a.SetorColonia)
                .ToListAsync();
        }

        [HttpGet("abertos")]
        public async Task<ActionResult<IEnumerable<AlertaColonia>>> GetAbertos()
        {
            return await _context.Alertas
                .Where(a => a.statusAlerta == StatusAlerta.Aberto)
                .Include(a => a.SetorColonia)
                .ToListAsync();
        }

        [HttpPut("{id:int}/finalizar")]
        public async Task<ActionResult<AlertaColonia>> Finalizar(int id)
        {
            try
            {
                var alerta = await _context.Alertas.FindAsync(id);

                if (alerta == null)
                {
                    throw new AlertaNaoEncontradoException("Alerta nao encontrado.");
                }

                alerta.statusAlerta = StatusAlerta.Finalizado;
                alerta.dtFinalizacao = DateTime.Now;

                await _context.SaveChangesAsync();

                return alerta;
            }
            catch (AlertaNaoEncontradoException ex)
            {
                _logger.LogWarning(ex, "Alerta nao encontrado.");
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var alerta = await _context.Alertas.FindAsync(id);

            if (alerta == null)
            {
                return NotFound("Alerta nao encontrado.");
            }

            var missoes = await _context.Missoes
                .Where(m => m.alertaColoniaId == id)
                .ToListAsync();

            _context.Missoes.RemoveRange(missoes);
            _context.Alertas.Remove(alerta);

            await _context.SaveChangesAsync();

            return Ok("Alerta removido com sucesso.");
        }
    }
}
