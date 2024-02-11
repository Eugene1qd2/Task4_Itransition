using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task4.Domain.Enums;
using Task4.Domain.Models;
using Task4.Domain.ViewModels.User;
using Task4.Domain.Response;

namespace Task4.Service.Interfaces
{
    public interface IUserService
    {
        Task<IBaseResponse<UsersViewModel>> GetUsers();
        Task<IBaseResponse<bool>> VerifyUser(string id);
        Task<IBaseResponse<User>> GetUserById(int id);
        Task<IBaseResponse<UserViewModel>> GetUserByName(string name);
        Task<IBaseResponse<bool>> DeleteUserById(int id);
        Task<IBaseResponse<bool>> SetUserStatus(int id, UserStatus status);
        Task<IBaseResponse<bool>> DeleteUsersById(List<int> ids);
        Task<IBaseResponse<bool>> SetUsersStatusById(List<int> ids,UserStatus status);

        List<int> GetUserIds(UsersViewModel usersViewModel);
    }
}
