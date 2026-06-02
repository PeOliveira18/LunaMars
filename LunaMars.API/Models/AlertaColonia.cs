using LunaMars.API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunaMars.API.Models
{
    public class AlertaColonia
    {
        [Key]
        public int idAlerta { get; set; }

        public string titulo { get; set; } = string.Empty;

        public string mensagem { get; set; } = string.Empty;

        public NivelRisco nivelRisco { get; set; }

        public StatusAlerta statusAlerta { get; set; }

        public DateTime dtCriacao { get; set; }

        public DateTime? dtFinalizacao { get; set; }

        public int setorColoniaId { get; set; }

        [ForeignKey("setorColoniaId")]
        public SetorColonia? SetorColonia { get; set; }

        public ICollection<MissaoResposta>? Missoes { get; set; }
    }
}
