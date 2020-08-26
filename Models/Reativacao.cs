using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancelamentoIdentity.Models
{
    public class Reativacao
    {
        public int IdCancelamento { get; set; }
        public string Email { get; set; }
        public string SenhaEmail { get; set; }
    }
}
