using LunaMars.API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunaMars.API.Models
{
    public class SensorAmbiental
    {
        [Key]
        public int idSensor { get; set; }

        public string nomeSensor { get; set; } = string.Empty;

        public TipoSensor tipoSensor { get; set; }

        public bool ativo { get; set; }

        public int setorColoniaId { get; set; }

        [ForeignKey("setorColoniaId")]
        public SetorColonia? SetorColonia { get; set; }

        public ICollection<LeituraSensor>? Leituras { get; set; }
    }
}
