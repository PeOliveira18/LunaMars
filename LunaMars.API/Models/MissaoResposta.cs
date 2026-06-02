using LunaMars.API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunaMars.API.Models
{
    public class MissaoResposta
    {
        [Key]
        public int idMissao { get; set; }

        public string descricao { get; set; } = string.Empty;

        public StatusMissao statusMissao { get; set; }

        public DateTime dtInicio { get; set; }

        public DateTime? dtFim { get; set; }

        public int alertaColoniaId { get; set; }

        [ForeignKey("alertaColoniaId")]
        public AlertaColonia? AlertaColonia { get; set; }
    }
}
