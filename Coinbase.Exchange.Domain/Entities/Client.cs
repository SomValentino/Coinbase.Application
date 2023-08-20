using System.ComponentModel.DataAnnotations;

namespace Coinbase.Exchange.Domain.Entities
{
    public class Client
    {
        
        [Key]
        public string ClientId { get; set; }
        [Required]
        public string ClientName { get; set; }
        public List<Instrument> ProductGroups { get; set; } = new();
        public ClientRegistration ClientRegistration { get; set; }
    }
}
