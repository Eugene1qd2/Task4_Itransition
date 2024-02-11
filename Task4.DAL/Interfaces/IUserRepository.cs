using Task4.Domain.Enums;
using Task4.Domain.Models;

namespace Task4.DAL.Interfaces
{
    public interface IUserRepository:IBaseRepository<User>
    {
        public Task<User> GetByUserName(string userName);
    }
}
