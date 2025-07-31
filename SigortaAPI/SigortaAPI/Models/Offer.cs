using System;

namespace SigortaAPI.Models
{
    public class Offer
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        // Navigation property, initialized to suppress nullable warning
        public Customer Customer { get; set; } = default!;

        public DateTime OfferDate { get; set; }

        public decimal PremiumAmount { get; set; }

        // Non-nullable string, initialized to suppress warning
        public string Status { get; set; } = default!;
    }
}
