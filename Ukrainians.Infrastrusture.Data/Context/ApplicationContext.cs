using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ukrainians.Infrastrusture.Data.Entities;

namespace Ukrainians.Infrastrusture.Data.Context
{
    public class ApplicationContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatNotification> ChatNotifications { get; set; }
        public DbSet<PushNotificationsSubscription> PushNotificationsSubscriptions { get; set; }
    }
}
