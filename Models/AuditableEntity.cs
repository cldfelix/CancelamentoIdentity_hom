using System;
using System.ComponentModel.DataAnnotations;

namespace CancelamentoIdentity.Models
{
    public class AuditableEntity
    {
        [MaxLength(256)]
        public string CriadoPor { get; set; }
        [MaxLength(256)]
        public string AtualizadoPor { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
