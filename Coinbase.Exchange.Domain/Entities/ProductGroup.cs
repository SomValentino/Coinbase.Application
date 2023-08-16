using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Entities
{
    public class ProductGroup
    {
        [Key]
        public string Instrument { get; set; }
        public ICollection<Client> Clients { get; set; }
    }
}
