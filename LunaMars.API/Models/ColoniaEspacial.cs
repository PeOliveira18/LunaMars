using LunaMars.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace LunaMars.API.Models
{
    public class ColoniaEspacial
    {
        [Key]
        public int idColonia { get; set; }

        public string nomeColonia { get; set; } = string.Empty;

        public TipoLocalizacao localizacao { get; set; }

        public DateTime dtCriacao { get; set; }

        public int capacidadePessoas { get; set; }

        public ICollection<SetorColonia>? Setores { get; set; }

        public ICollection<RecursoVital>? Recursos { get; set; }
    }
}
