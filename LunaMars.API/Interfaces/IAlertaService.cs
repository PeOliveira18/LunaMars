using LunaMars.API.Models;

namespace LunaMars.API.Interfaces
{
    public interface IAlertaService
    {
        Task<AlertaColonia?> GerarAlertaSeNecessario(LeituraSensor leitura);
    }
}
