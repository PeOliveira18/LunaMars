using LunaMars.API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunaMars.API.Models
{
    public class SetorColonia
    {
        [Key]
        public int idSetor { get; set; }

        public string nomeSetor { get; set; } = string.Empty;

        public TipoSetor tipoSetor { get; set; }

        public double pressaoInterna { get; set; }

        public double temperaturaAtual { get; set; }

        public bool ativo { get; set; }

        public int coloniaEspacialId { get; set; }

        [ForeignKey("coloniaEspacialId")]
        public ColoniaEspacial? ColoniaEspacial { get; set; }

        public ICollection<SensorAmbiental>? Sensores { get; set; }

        public ICollection<LeituraSensor>? Leituras { get; set; }
    }
}
