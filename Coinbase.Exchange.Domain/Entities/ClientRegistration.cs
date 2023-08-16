using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Entities
{
    public class ClientRegistration
    {
        [Required]
        [Key]
        public string ClientId { get; set; }
        [Required]
        public string TokenId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public bool Active { get; set; }
        public Client Client { get; set; }
    }
}
