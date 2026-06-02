namespace LunaMars.API.Models
{
    public class SateliteOrbital : EquipamentoEspacial
    {
        public string orbita { get; set; } = string.Empty;

        public string agenciaOperadora { get; set; } = string.Empty;

        public override string ObterFuncao()
        {
            return "Monitoramento orbital e comunicacao com a colonia.";
        }
    }
}
