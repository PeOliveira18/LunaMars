namespace LunaMars.API.Models
{
    public class RoverExploracao : EquipamentoEspacial
    {
        public double nivelBateria { get; set; }

        public double distanciaPercorridaKm { get; set; }

        public override string ObterFuncao()
        {
            return "Exploracao da superficie e coleta de dados ambientais.";
        }
    }
}
