using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.ApiDto
{
    public class AuthRequest
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientName { get; set; }
    }
}
