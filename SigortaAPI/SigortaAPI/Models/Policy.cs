using System;

namespace SigortaAPI.Models
{
    public class Policy
    {
        public int Id { get; set; }

        public int OfferId { get; set; }

        // Navigation property, initialized to suppress nullable warning
        public Offer Offer { get; set; } = default!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        // Non-nullable string, initialized to suppress warning
        public string Status { get; set; } = default!;
    }
}
