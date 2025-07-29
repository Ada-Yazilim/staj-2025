using System;

namespace SigortaAPI.Models
{
    public class Document
    {
        public int Id { get; set; }

        // Suppress nullable warning
        public string FileName { get; set; } = default!;

        public string FileType { get; set; } = default!;

        public DateTime UploadedAt { get; set; }

        // The ID of the user who uploaded the document
        public string UploadedById { get; set; } = default!;

        // Navigation property for the uploading user
        public User UploadedBy { get; set; } = default!;

        /// <summary>
        /// 1 = Customer, 2 = Policy, 3 = Claim (you might prefer an enum here)
        /// </summary>
        public int RelatedType { get; set; }

        // The primary key of the related entity (Customer.Id, Policy.Id, or Claim.Id)
        public int RelatedId { get; set; }
    }
}
