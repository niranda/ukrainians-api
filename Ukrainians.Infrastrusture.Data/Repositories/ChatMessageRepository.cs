using Microsoft.EntityFrameworkCore;
using Ukrainians.Infrastrusture.Data.Context;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.Infrastrusture.Data.Stores;

namespace Ukrainians.Infrastrusture.Data.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ApplicationContext _context;

        public ChatMessageRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatMessage>> GetAll()
        {
            return await _context.ChatMessages
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Created)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetAllByRoomId(Guid roomId)
        {
            return await _context.ChatMessages
                .Where(x => !x.IsDeleted && x.ChatRoomId == roomId)
                .OrderBy(x => x.Created)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ChatMessage?> GetById(Guid id, bool asNoTracking = true)
        {
            IQueryable<ChatMessage> query = _context.ChatMessages;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<ChatMessage> Create(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<ChatMessage> Update(ChatMessage message)
        {
            _context.ChatMessages.Update(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<IEnumerable<ChatMessage>> UpdateRange(IEnumerable<ChatMessage> messages)
        {
            _context.ChatMessages.UpdateRange(messages);
            await _context.SaveChangesAsync();
            return messages;
        }

        public async Task<bool> Delete(ChatMessage message)
        {
            _context.ChatMessages.Remove(message);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
