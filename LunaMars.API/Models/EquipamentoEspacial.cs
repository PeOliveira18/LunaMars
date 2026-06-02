using System.ComponentModel.DataAnnotations;

namespace LunaMars.API.Models
{
    public abstract class EquipamentoEspacial
    {
        [Key]
        public int idEquipamento { get; set; }

        public string nomeEquipamento { get; set; } = string.Empty;

        public DateTime dtAtivacao { get; set; }

        public bool ativo { get; set; }

        public abstract string ObterFuncao();
    }
}
