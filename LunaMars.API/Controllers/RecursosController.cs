using LunaMars.API.Data;
using LunaMars.API.Exceptions;
using LunaMars.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunaMars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecursosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecursoVital>>> Get()
        {
            return await _context.Recursos
                .Include(r => r.ColoniaEspacial)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RecursoVital>> Get(int id)
        {
            var recurso = await _context.Recursos.FindAsync(id);

            if (recurso == null)
            {
                return NotFound("Recurso nao encontrado.");
            }

            return recurso;
        }

        [HttpGet("colonia/{coloniaId:int}")]
        public async Task<ActionResult<IEnumerable<RecursoVital>>> GetPorColonia(int coloniaId)
        {
            return await _context.Recursos
                .Where(r => r.coloniaEspacialId == coloniaId)
                .ToListAsync();
        }

        [HttpGet("criticos")]
        public async Task<ActionResult<IEnumerable<RecursoVital>>> GetCriticos()
        {
            var recursos = await _context.Recursos.ToListAsync();

            return recursos.Where(r => r.EstaCritico()).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<RecursoVital>> Post(RecursoVital recurso)
        {
            try
            {
                ValidarRecurso(recurso);

                var colonia = await _context.Colonias.FindAsync(recurso.coloniaEspacialId);

                if (colonia == null)
                {
                    throw new RecursoNaoEncontradoException("Colonia do recurso nao encontrada.");
                }

                _context.Recursos.Add(recurso);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = recurso.idRecurso }, recurso);
            }
            catch (RecursoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RecursoInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var recurso = await _context.Recursos.FindAsync(id);

            if (recurso == null)
            {
                return NotFound("Recurso nao encontrado.");
            }

            _context.Recursos.Remove(recurso);
            await _context.SaveChangesAsync();

            return Ok("Recurso removido com sucesso.");
        }

        private void ValidarRecurso(RecursoVital recurso)
        {
            if (recurso.quantidade < 0)
            {
                throw new RecursoInvalidoException("A quantidade do recurso nao pode ser negativa.");
            }

            if (recurso.nivelMinimo < 0)
            {
                throw new RecursoInvalidoException("O nivel minimo do recurso nao pode ser negativo.");
            }

            if (string.IsNullOrWhiteSpace(recurso.unidadeMedida))
            {
                throw new RecursoInvalidoException("A unidade de medida do recurso e obrigatoria.");
            }

            if (recurso.dtValidade.HasValue && recurso.dtValidade.Value.Date < DateTime.Now.Date)
            {
                throw new RecursoInvalidoException("A validade do recurso nao pode estar vencida.");
            }
        }
    }
}
