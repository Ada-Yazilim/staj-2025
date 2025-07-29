using System;

namespace SigortaAPI.Models
{
    public class Claim
    {
        public int Id { get; set; }

        public int PolicyId { get; set; }

        // Navigation property, initialized to suppress nullability warning
        public Policy Policy { get; set; } = default!;

        public DateTime ClaimDate { get; set; }

        // Non-nullable string, initialized to suppress warning
        public string Status { get; set; } = default!;

        // Non-nullable string, initialized to suppress warning
        public string Description { get; set; } = default!;
    }
}
