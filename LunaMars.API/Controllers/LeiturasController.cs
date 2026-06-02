using LunaMars.API.Data;
using LunaMars.API.Enums;
using LunaMars.API.Exceptions;
using LunaMars.API.Interfaces;
using LunaMars.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunaMars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeiturasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICalculadoraRisco _calculadoraRisco;
        private readonly IAlertaService _alertaService;
        private readonly ILogger<LeiturasController> _logger;

        public LeiturasController(
            AppDbContext context,
            ICalculadoraRisco calculadoraRisco,
            IAlertaService alertaService,
            ILogger<LeiturasController> logger)
        {
            _context = context;
            _calculadoraRisco = calculadoraRisco;
            _alertaService = alertaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeituraSensor>>> Get()
        {
            return await _context.Leituras
                .Include(l => l.SensorAmbiental)
                .Include(l => l.SetorColonia)
                .ToListAsync();
        }

        [HttpGet("setor/{setorId:int}")]
        public async Task<ActionResult<IEnumerable<LeituraSensor>>> GetPorSetor(int setorId)
        {
            return await _context.Leituras
                .Where(l => l.setorColoniaId == setorId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(LeituraSensor leitura)
        {
            try
            {
                ValidarDadosBasicos(leitura);

                var setor = await _context.Setores.FindAsync(leitura.setorColoniaId);

                if (setor == null)
                {
                    throw new SetorNaoEncontradoException("Setor nao encontrado.");
                }

                var sensor = await _context.Sensores.FindAsync(leitura.sensorAmbientalId);

                if (sensor == null)
                {
                    throw new SensorNaoEncontradoException("Sensor nao encontrado.");
                }

                if (!sensor.ativo)
                {
                    throw new LeituraInvalidaException("Sensor inativo. Nao e possivel registrar leitura.");
                }

                if (sensor.setorColoniaId != setor.idSetor)
                {
                    throw new LeituraInvalidaException("Sensor nao pertence ao setor informado.");
                }

                leitura.tipoSensor = sensor.tipoSensor;
                leitura.dtLeitura = DateTime.Now;

                var risco = _calculadoraRisco.CalcularRisco(leitura);

                _context.Leituras.Add(leitura);
                await _context.SaveChangesAsync();

                var alerta = await _alertaService.GerarAlertaSeNecessario(leitura);

                return Ok(new
                {
                    leituraId = leitura.idLeitura,
                    tipoSensor = leitura.tipoSensor.ToString(),
                    valor = leitura.valor,
                    unidadeMedida = leitura.unidadeMedida,
                    dataLeitura = leitura.dtLeitura,
                    nivelRisco = risco.ToString(),
                    alertaGerado = alerta != null,
                    alertaId = alerta?.idAlerta,
                    mensagem = alerta == null
                        ? "Leitura registrada sem alerta."
                        : $"Alerta {alerta.nivelRisco} gerado para o setor {setor.nomeSetor}."
                });
            }
            catch (LeituraInvalidaException ex)
            {
                _logger.LogWarning(ex, "Leitura invalida recebida.");
                return BadRequest(ex.Message);
            }
            catch (SetorNaoEncontradoException ex)
            {
                _logger.LogWarning(ex, "Setor nao encontrado.");
                return NotFound(ex.Message);
            }
            catch (SensorNaoEncontradoException ex)
            {
                _logger.LogWarning(ex, "Sensor nao encontrado.");
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar leitura no banco.");
                return StatusCode(500, "Erro ao salvar leitura no banco de dados.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var leitura = await _context.Leituras.FindAsync(id);

            if (leitura == null)
            {
                return NotFound("Leitura nao encontrada.");
            }

            _context.Leituras.Remove(leitura);
            await _context.SaveChangesAsync();

            return Ok("Leitura removida com sucesso.");
        }

        private void ValidarDadosBasicos(LeituraSensor leitura)
        {
            if (leitura.valor < 0)
            {
                throw new LeituraInvalidaException("O valor da leitura nao pode ser negativo.");
            }

            if (string.IsNullOrWhiteSpace(leitura.unidadeMedida))
            {
                throw new LeituraInvalidaException("A unidade de medida da leitura e obrigatoria.");
            }
        }
    }
}
