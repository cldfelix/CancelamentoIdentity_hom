using System.ComponentModel.DataAnnotations;

namespace CancelamentoIdentity.Models
{

    public class Anexo
    {

        [Key]
        public int Id { get; set; }
        public string NomeOriginal {get; set;}
        public string NomeCriado { get; set; }
        public virtual Cancelamento Cancelamento { get; set; }
    }
}
