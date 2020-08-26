using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CancelamentoIdentity.Models
{
    public class TiposCancelamentos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string CodAdm { get; set; }
        public string Tipo { get; set; }
        public string TipoSistemaCancelamento { get; set; }
        public bool Abonavel { get; set; }
        public string Descricao { get; set; }
    }
}
