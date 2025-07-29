using Microsoft.AspNetCore.Identity;

namespace SigortaAPI.Models
{
    public class User : IdentityUser
    {
        /// <summary>
        /// Eğer bu User bir müşteri portal hesabı ise, hangi Customer kaydına ait olduğunu belirtir.
        /// İç ekip (Agent/Admin) hesapları için null kalabilir.
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// İsteğe bağlı navigation property: Bu kullanıcıya ait Customer profili.
        /// </summary>
        public Customer? Customer { get; set; }
    }
}
