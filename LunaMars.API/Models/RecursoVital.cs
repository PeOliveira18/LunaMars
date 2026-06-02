using LunaMars.API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunaMars.API.Models
{
    public class RecursoVital
    {
        [Key]
        public int idRecurso { get; set; }

        public TipoRecurso tipoRecurso { get; set; }

        public double quantidade { get; set; }

        public string unidadeMedida { get; set; } = string.Empty;

        public double nivelMinimo { get; set; }

        public DateTime? dtValidade { get; set; }

        public int coloniaEspacialId { get; set; }

        [ForeignKey("coloniaEspacialId")]
        public ColoniaEspacial? ColoniaEspacial { get; set; }

        public bool EstaCritico()
        {
            return quantidade <= nivelMinimo;
        }
    }
}
