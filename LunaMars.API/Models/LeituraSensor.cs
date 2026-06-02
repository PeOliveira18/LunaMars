using LunaMars.API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunaMars.API.Models
{
    public class LeituraSensor
    {
        [Key]
        public int idLeitura { get; set; }

        public TipoSensor tipoSensor { get; set; }

        public double valor { get; set; }

        public string unidadeMedida { get; set; } = string.Empty;

        public DateTime dtLeitura { get; set; }

        public int sensorAmbientalId { get; set; }

        [ForeignKey("sensorAmbientalId")]
        public SensorAmbiental? SensorAmbiental { get; set; }

        public int setorColoniaId { get; set; }

        [ForeignKey("setorColoniaId")]
        public SetorColonia? SetorColonia { get; set; }
    }
}
