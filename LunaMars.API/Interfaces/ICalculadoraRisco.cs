using LunaMars.API.Enums;
using LunaMars.API.Models;

namespace LunaMars.API.Interfaces
{
    public interface ICalculadoraRisco
    {
        NivelRisco CalcularRisco(LeituraSensor leitura);
    }
}
