using Microsoft.EntityFrameworkCore;
using Ukrainians.Infrastrusture.Data.Context;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.Infrastrusture.Data.Stores;

namespace Ukrainians.Infrastrusture.Data.Repositories
{
    public class PushNotificationsSubscriptionRepository : IPushNotificationsSubscriptionRepository
    {
        private readonly ApplicationContext _context;

        public PushNotificationsSubscriptionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PushNotificationsSubscription>> GetAll()
        {
            return await _context.PushNotificationsSubscriptions
                .AsNoTracking()
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<PushNotificationsSubscription?> GetById(Guid id, bool asNoTracking = true)
        {
            IQueryable<PushNotificationsSubscription> query = _context.PushNotificationsSubscriptions;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<PushNotificationsSubscription?> GetByUsername(string username)
        {
            return await _context.PushNotificationsSubscriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => !s.IsDeleted && s.Username == username);
        }

        public async Task<PushNotificationsSubscription> Create(PushNotificationsSubscription pushNotificationsSubscription)
        {
            await _context.PushNotificationsSubscriptions.AddAsync(pushNotificationsSubscription);
            await _context.SaveChangesAsync();

            return pushNotificationsSubscription;
        }

        public async Task<bool> Delete(PushNotificationsSubscription pushNotificationsSubscription)
        {
            pushNotificationsSubscription.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
