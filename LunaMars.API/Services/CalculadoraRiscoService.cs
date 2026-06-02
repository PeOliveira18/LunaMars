using LunaMars.API.Enums;
using LunaMars.API.Exceptions;
using LunaMars.API.Interfaces;
using LunaMars.API.Models;

namespace LunaMars.API.Services
{
    public class CalculadoraRiscoService : ICalculadoraRisco
    {
        public NivelRisco CalcularRisco(LeituraSensor leitura)
        {
            ValidarLeitura(leitura);

            switch (leitura.tipoSensor)
            {
                case TipoSensor.Oxigenio:
                    return CalcularOxigenio(leitura.valor);
                case TipoSensor.Pressao:
                    return CalcularPressao(leitura.valor);
                case TipoSensor.Temperatura:
                    return CalcularTemperatura(leitura.valor);
                case TipoSensor.Radiacao:
                    return CalcularRadiacao(leitura.valor);
                case TipoSensor.Energia:
                    return CalcularEnergia(leitura.valor);
                default:
                    return NivelRisco.Baixo;
            }
        }

        private void ValidarLeitura(LeituraSensor leitura)
        {
            if (leitura.valor < 0)
            {
                throw new LeituraInvalidaException("O valor da leitura nao pode ser negativo.");
            }

            if (string.IsNullOrWhiteSpace(leitura.unidadeMedida))
            {
                throw new LeituraInvalidaException("A unidade de medida da leitura e obrigatoria.");
            }

            if (leitura.tipoSensor == TipoSensor.Oxigenio && leitura.valor > 100)
            {
                throw new LeituraInvalidaException("O percentual de oxigenio nao pode ser maior que 100.");
            }

            if (leitura.tipoSensor == TipoSensor.Energia && leitura.valor > 100)
            {
                throw new LeituraInvalidaException("O percentual de energia nao pode ser maior que 100.");
            }

            if (leitura.tipoSensor == TipoSensor.Temperatura && (leitura.valor < -150 || leitura.valor > 100))
            {
                throw new LeituraInvalidaException("A temperatura informada esta fora do intervalo permitido.");
            }

            if (leitura.tipoSensor == TipoSensor.Pressao && leitura.valor > 200)
            {
                throw new LeituraInvalidaException("A pressao informada esta fora do intervalo permitido.");
            }

            if (leitura.tipoSensor == TipoSensor.Radiacao && leitura.valor > 1000)
            {
                throw new LeituraInvalidaException("A radiacao informada esta fora do intervalo permitido.");
            }
        }

        private NivelRisco CalcularOxigenio(double valor)
        {
            if (valor < 16) return NivelRisco.Critico;
            if (valor < 19.5) return NivelRisco.Alto;
            if (valor < 21) return NivelRisco.Medio;

            return NivelRisco.Baixo;
        }

        private NivelRisco CalcularPressao(double valor)
        {
            if (valor < 70) return NivelRisco.Critico;
            if (valor < 85) return NivelRisco.Alto;
            if (valor < 95) return NivelRisco.Medio;

            return NivelRisco.Baixo;
        }

        private NivelRisco CalcularTemperatura(double valor)
        {
            if (valor < -10 || valor > 45) return NivelRisco.Critico;
            if (valor < 0 || valor > 35) return NivelRisco.Alto;
            if (valor < 10 || valor > 30) return NivelRisco.Medio;

            return NivelRisco.Baixo;
        }

        private NivelRisco CalcularRadiacao(double valor)
        {
            if (valor > 80) return NivelRisco.Critico;
            if (valor > 50) return NivelRisco.Alto;
            if (valor > 30) return NivelRisco.Medio;

            return NivelRisco.Baixo;
        }

        private NivelRisco CalcularEnergia(double valor)
        {
            if (valor < 20) return NivelRisco.Critico;
            if (valor < 40) return NivelRisco.Alto;
            if (valor < 60) return NivelRisco.Medio;

            return NivelRisco.Baixo;
        }
    }
}
