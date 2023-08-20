using System.ComponentModel.DataAnnotations;

namespace Coinbase.Exchange.Domain.Entities
{
    public class Instrument
    {
        public Instrument()
        {
            Clients = new List<Client>();
        }
        [Key]
        public string Name { get; set; }
        public List<Client> Clients { get; set; } = new();
    }
}
