using System;

namespace SigortaAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int PolicyId { get; set; }

        // Navigation property, initialized to suppress nullable warning
        public Policy Policy { get; set; } = default!;

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        // Non-nullable string, initialized to suppress warning
        public string Method { get; set; } = default!;
    }
}
