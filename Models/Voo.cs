using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CancelamentoIdentity.Models
{
    public class VooForm
    {
        [Display(Name = "Entre com o numero voo cancelado ex. '1001'")]
        public string NumeroDoVoo { get; set; }
        
        [Display(Name = "Entre com data do voo cancelado")]
        [Range(typeof(DateTime), "01/01/2015", "01/01/2025",
        ErrorMessage = "Valor de {0} deve ser entre {1} e {2}")]
        public DateTime DataDoVoo { get; set; }
    }

    public class Voo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Data do voo é requerida!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data do voo cancelado")]
        [DataType(DataType.Date)]
        public DateTime DataVoo { get; set; }
        [Display(Name = "Número voo cancelado ex. '1001'")]
        [Required(ErrorMessage = "Digite um numero de voo válido!")]
        public string NumeroDoVoo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:ss}")]
        [Display(Name = "STA - UTC")]
        // [Display(Name = "Standard Time Arrival")]
        public DateTime STA { get; set; }

        [Display(Name = "STD - UTC")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:ss}")]
        public DateTime STD { get; set; }

        [Display(Name = "Origem do Voo")]
        public string Origem { get; set; }

        [Display(Name = "Destino do Voo")]
        public string Destino { get; set; }

        [Display(Name = "Quantidade de passageiros no voo")]
        [Range(0, 194, ErrorMessage="Qtd. válida está entre 0 e 194 passageiros")]
        public ushort QtdPassageiros { get; set; }

        [Display(Name = "Metar de Origem")]
        public string MetarOrigem { get; set; }
        [Display(Name = "Metar de Destino")]

        public string MetarDestino { get; set; }

        [Display(Name = "Matrícula do voo")]
        [MaxLength(10)]
        public string Matricula { get; set; }
        [MaxLength(50)]

        [Display(Name = "Tipo do Voo")]
        public string TipoVoo { get; set; }

    }


}
