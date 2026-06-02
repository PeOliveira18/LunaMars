using LunaMars.API.Data;
using LunaMars.API.Exceptions;
using LunaMars.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunaMars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipamentosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("rovers")]
        public async Task<ActionResult<IEnumerable<RoverExploracao>>> GetRovers()
        {
            return await _context.Rovers.ToListAsync();
        }

        [HttpPost("rovers")]
        public async Task<ActionResult<RoverExploracao>> PostRover(RoverExploracao rover)
        {
            try
            {
                ValidarRover(rover);

                rover.dtAtivacao = DateTime.Now;
                rover.ativo = true;

                _context.Rovers.Add(rover);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRovers), new { id = rover.idEquipamento }, rover);
            }
            catch (EquipamentoInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("rovers/{id:int}")]
        public async Task<ActionResult> DeleteRover(int id)
        {
            var rover = await _context.Rovers.FindAsync(id);

            if (rover == null)
            {
                return NotFound("Rover nao encontrado.");
            }

            _context.Rovers.Remove(rover);
            await _context.SaveChangesAsync();

            return Ok("Rover removido com sucesso.");
        }

        [HttpGet("satelites")]
        public async Task<ActionResult<IEnumerable<SateliteOrbital>>> GetSatelites()
        {
            return await _context.Satelites.ToListAsync();
        }

        [HttpPost("satelites")]
        public async Task<ActionResult<SateliteOrbital>> PostSatelite(SateliteOrbital satelite)
        {
            try
            {
                ValidarSatelite(satelite);

                satelite.dtAtivacao = DateTime.Now;
                satelite.ativo = true;

                _context.Satelites.Add(satelite);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSatelites), new { id = satelite.idEquipamento }, satelite);
            }
            catch (EquipamentoInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("satelites/{id:int}")]
        public async Task<ActionResult> DeleteSatelite(int id)
        {
            var satelite = await _context.Satelites.FindAsync(id);

            if (satelite == null)
            {
                return NotFound("Satelite nao encontrado.");
            }

            _context.Satelites.Remove(satelite);
            await _context.SaveChangesAsync();

            return Ok("Satelite removido com sucesso.");
        }

        private void ValidarRover(RoverExploracao rover)
        {
            if (string.IsNullOrWhiteSpace(rover.nomeEquipamento))
            {
                throw new EquipamentoInvalidoException("O nome do rover e obrigatorio.");
            }

            if (rover.nivelBateria < 0 || rover.nivelBateria > 100)
            {
                throw new EquipamentoInvalidoException("O nivel de bateria deve estar entre 0 e 100.");
            }

            if (rover.distanciaPercorridaKm < 0)
            {
                throw new EquipamentoInvalidoException("A distancia percorrida nao pode ser negativa.");
            }
        }

        private void ValidarSatelite(SateliteOrbital satelite)
        {
            if (string.IsNullOrWhiteSpace(satelite.nomeEquipamento))
            {
                throw new EquipamentoInvalidoException("O nome do satelite e obrigatorio.");
            }

            if (string.IsNullOrWhiteSpace(satelite.orbita))
            {
                throw new EquipamentoInvalidoException("A orbita do satelite e obrigatoria.");
            }

            if (string.IsNullOrWhiteSpace(satelite.agenciaOperadora))
            {
                throw new EquipamentoInvalidoException("A agencia operadora do satelite e obrigatoria.");
            }
        }
    }
}
