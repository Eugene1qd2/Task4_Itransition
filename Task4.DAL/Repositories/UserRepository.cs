using Microsoft.EntityFrameworkCore;
using Task4.DAL.Interfaces;
using Task4.Domain.Enums;
using Task4.Domain.Models;

namespace Task4.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(User obj)
        {
            if (obj == null)
                return false;
            await _db.Users.AddAsync(obj);
            return await _db.SaveChangesAsync() >= 0;
        }

        public async Task<bool> Delete(User obj)
        {
            if (obj == null)
                return false;
            _db.Users.Remove(obj);
            return await _db.SaveChangesAsync() >= 0;
        }

        public async Task<List<User>> GetAll()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetByUserName(string userName)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<bool> Update(User obj)
        {
            if (obj == null)
                return false;
            obj.Updated = DateTime.Now;
            _db.Users.Update(obj);
            return await _db.SaveChangesAsync() >= 0;
        }
    }
}
