using System.ComponentModel.DataAnnotations;
using Ukrainians.Infrastrusture.Data.Entities.Base;

namespace Ukrainians.Infrastrusture.Data.Entities
{
    public class PushNotificationsSubscription : BaseEntity
    {
        [Required]
        public string Endpoint { get; set; }

        [Required]
        public string P256DH { get; set; }

        [Required]
        public string Auth { get; set; }

        [StringLength(50)]
        public string? Username { get; set; }
    }
}
