using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CancelamentoIdentity.Models
{

    public class Cancelamento : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

   
        [Required]
        public virtual TiposCancelamentos TipoCancelamento { get; set; }

        [Required]
        public virtual Voo VooCancelado { get; set; }
        public virtual Voo VooAnterior { get; set; }

        [UIHint("tinymce_jquery_full")]
        [Display(Name = "Observações")]
        public string Observacao { get; set; }

        [Required(ErrorMessage = "Voo Cancelado/Alternado é requerido")]
        [Display(Name = "Voo é cancelado?")]
        public bool Cancelado { get; set; }

        [Display(Name = "Adicionar documento")]
        public virtual List<Anexo> Anexos { get; set; }

        [NotMapped]
        [Display(Name ="Enviar email sobre cancelamento?")]
        public bool  EnviarEmail { get; set; }

        [NotMapped]
        [Display(Name = "Informe o seu email")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Informe um email válido")]
        public string EmailUsuario { get; set; }
        
        [NotMapped]
        [Display(Name = "Digite a senha do seu email")]
        [DataType(DataType.Password, ErrorMessage ="Informe uma senha")]
        public string SenhaUsuario { get; set; }

        [NotMapped]
        public virtual Anexo Anexo { get; set; }

        [Required(ErrorMessage = "Campo alvo de Processo é obrigatorio!")]

        public bool AlvoDeProcesso { get; set; }

        public bool Reativado { get; set; }


    }
    public class Filter
    {
        public string FilterName { get; set; }
        public int Id { get; set; }
    }
}
