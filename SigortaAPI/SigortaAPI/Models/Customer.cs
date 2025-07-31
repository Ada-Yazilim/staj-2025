using System.Collections.Generic;

namespace SigortaAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }

        // Initialize Name to suppress nullable warnings
        public string Name { get; set; } = default!;

        // Navigation property: a customer can have multiple offers
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
