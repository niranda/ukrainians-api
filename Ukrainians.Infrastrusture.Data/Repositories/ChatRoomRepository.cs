using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Ukrainians.Infrastrusture.Data.Context;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.Infrastrusture.Data.Stores;

namespace Ukrainians.Infrastrusture.Data.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly ApplicationContext _context;

        public ChatRoomRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatRoom>> GetAll()
        {
            return await _context.ChatRooms
                .Where(s => !s.IsDeleted)
                .Include(s => s.ChatMessages)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ChatRoom?> GetById(Guid id, bool asNoTracking = true)
        {
            IQueryable<ChatRoom> query = _context.ChatRooms;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query
                .Include(s => s.ChatMessages.OrderBy(s => s.Created))
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<ChatRoom>> GetRoomsUserInteractedWith(string username)
        {
            return await _context.ChatRooms
                .Where(s => !s.IsDeleted && s.RoomName != null && s.RoomName.Contains(username))
                .Include(s => s.ChatMessages.OrderByDescending(s => s.Created))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ChatRoom?> GetByName(string name, bool asNoTracking = true)
        {
            IQueryable<ChatRoom> query = _context.ChatRooms;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query
                .Include(s => s.ChatMessages)
                .Include(s => s.Notifications)
                .FirstOrDefaultAsync(s => s.RoomName == name);
        }

        public async Task<ChatRoom> Create(ChatRoom room)
        {
            await _context.ChatRooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<ChatRoom> Update(ChatRoom room)
        {
            _context.ChatRooms.Update(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<bool> Delete(ChatRoom room)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Удаление уведомлений, связанных с комнатой
                    var notifications = _context.ChatNotifications.Where(n => n.ChatRoomId == room.Id);
                    _context.ChatNotifications.RemoveRange(notifications);

                    // Удаление сообщений из чата
                    _context.ChatMessages.RemoveRange(room.ChatMessages);

                    // Удаление комнаты
                    _context.ChatRooms.Remove(room);

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    // Здесь можно добавить логирование ошибки
                    Console.WriteLine($"Ошибка при удалении комнаты: {ex.Message}");

                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
