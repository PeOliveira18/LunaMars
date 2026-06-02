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
    public class SensoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SensoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorAmbiental>>> Get()
        {
            return await _context.Sensores
                .Include(s => s.SetorColonia)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SensorAmbiental>> Get(int id)
        {
            var sensor = await _context.Sensores.FindAsync(id);

            if (sensor == null)
            {
                return NotFound("Sensor nao encontrado.");
            }

            return sensor;
        }

        [HttpGet("setor/{setorId:int}")]
        public async Task<ActionResult<IEnumerable<SensorAmbiental>>> GetPorSetor(int setorId)
        {
            return await _context.Sensores
                .Where(s => s.setorColoniaId == setorId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<SensorAmbiental>> Post(SensorAmbiental sensor)
        {
            try
            {
                ValidarSensor(sensor);

                var setor = await _context.Setores.FindAsync(sensor.setorColoniaId);

                if (setor == null)
                {
                    return NotFound("Setor nao encontrado.");
                }

                sensor.ativo = true;

                _context.Sensores.Add(sensor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = sensor.idSensor }, sensor);
            }
            catch (SensorInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var sensor = await _context.Sensores.FindAsync(id);

            if (sensor == null)
            {
                return NotFound("Sensor nao encontrado.");
            }

            var leituras = await _context.Leituras
                .Where(l => l.sensorAmbientalId == id)
                .ToListAsync();

            _context.Leituras.RemoveRange(leituras);
            _context.Sensores.Remove(sensor);

            await _context.SaveChangesAsync();

            return Ok("Sensor removido com sucesso.");
        }

        private void ValidarSensor(SensorAmbiental sensor)
        {
            if (string.IsNullOrWhiteSpace(sensor.nomeSensor))
            {
                throw new SensorInvalidoException("O nome do sensor e obrigatorio.");
            }

            if (!Enum.IsDefined(typeof(TipoSensor), sensor.tipoSensor))
            {
                throw new SensorInvalidoException("O tipo do sensor informado e invalido.");
            }
        }
    }
}
