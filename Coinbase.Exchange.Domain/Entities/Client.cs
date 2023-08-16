using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Entities
{
    public class Client
    {
        [Key]
        public string ClientId { get; set; }
        [Required]
        public string ClientName { get; set; }
        public ICollection<ProductGroup> ProductGroups { get; set; }
    }
}
