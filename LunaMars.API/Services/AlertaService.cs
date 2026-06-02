using LunaMars.API.Data;
using LunaMars.API.Enums;
using LunaMars.API.Exceptions;
using LunaMars.API.Interfaces;
using LunaMars.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LunaMars.API.Services
{
    public class AlertaService : IAlertaService
    {
        private readonly AppDbContext _context;
        private readonly ICalculadoraRisco _calculadoraRisco;

        public AlertaService(AppDbContext context, ICalculadoraRisco calculadoraRisco)
        {
            _context = context;
            _calculadoraRisco = calculadoraRisco;
        }

        public async Task<AlertaColonia?> GerarAlertaSeNecessario(LeituraSensor leitura)
        {
            var risco = _calculadoraRisco.CalcularRisco(leitura);

            if (risco != NivelRisco.Alto && risco != NivelRisco.Critico)
            {
                return null;
            }

            var setor = await _context.Setores.FindAsync(leitura.setorColoniaId);

            if (setor == null)
            {
                throw new SetorNaoEncontradoException("Setor nao encontrado.");
            }

            var alertaAberto = await _context.Alertas
                .FirstOrDefaultAsync(a =>
                    a.setorColoniaId == leitura.setorColoniaId &&
                    a.statusAlerta == StatusAlerta.Aberto &&
                    a.nivelRisco == risco &&
                    a.titulo == MontarTitulo(leitura.tipoSensor));

            if (alertaAberto != null)
            {
                return alertaAberto;
            }

            var alerta = new AlertaColonia
            {
                titulo = MontarTitulo(leitura.tipoSensor),
                mensagem = $"Leitura critica detectada no setor {setor.nomeSetor}. Valor: {leitura.valor} {leitura.unidadeMedida}.",
                nivelRisco = risco,
                statusAlerta = StatusAlerta.Aberto,
                dtCriacao = DateTime.Now,
                setorColoniaId = setor.idSetor
            };

            _context.Alertas.Add(alerta);
            await _context.SaveChangesAsync();

            return alerta;
        }

        private string MontarTitulo(TipoSensor tipoSensor)
        {
            switch (tipoSensor)
            {
                case TipoSensor.Oxigenio:
                    return "Falha no oxigenio";
                case TipoSensor.Pressao:
                    return "Baixa pressao";
                case TipoSensor.Temperatura:
                    return "Temperatura extrema";
                case TipoSensor.Radiacao:
                    return "Risco de radiacao";
                case TipoSensor.Energia:
                    return "Energia critica";
                default:
                    return "Alerta da colonia";
            }
        }
    }
}
