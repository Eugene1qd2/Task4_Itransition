using System;
using System.Collections.Generic;
using System.Linq;
using Task4.Domain.Models;
using Task4.Domain.Response;
using Task4.Service.Interfaces;
using Task4.DAL.Interfaces;
using Task4.Domain.Enums;
using Task4.Domain.ViewModels.User;

namespace Task4.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IBaseResponse<bool>> VerifyUser(string username)
        {
            var baseResponse = await GetUserByName(username);
            if (baseResponse.StatusCode == StatusCode.OK)
            {
                var user = baseResponse.Data;
                if (user.Status == UserStatus.Active)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "[VeriyUser] : User verified successfully!",
                        Data = true,
                        StatusCode = StatusCode.OK,
                    };

                }
            }
            return new BaseResponse<bool>()
            {
                Data = false,
                Description = "[VerifyUser] : User blocked!",
                StatusCode = StatusCode.USER_BLOCKED,
            };
        }

        public async Task<IBaseResponse<User>> GetUserById(int id)
        {
            var baseResponse = new BaseResponse<User>();
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                {
                    baseResponse.Description = $"[GetUserById] : User not found!";
                    baseResponse.StatusCode = StatusCode.USER_NOT_FOUND;
                    return baseResponse;
                }
                baseResponse.Data = user;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = $"[GetUserById] : {ex.Message}",
                    StatusCode = StatusCode.SERVER_ERROR,
                };
            }
        }
        public async Task<IBaseResponse<UsersViewModel>> GetUsers()
        {
            var baseResponse = new BaseResponse<UsersViewModel>();
            UsersViewModel usersViewModel = new UsersViewModel();
            usersViewModel.Users = new List<UserViewModel>();
            try
            {
                var users = await _userRepository.GetAll();
                baseResponse.Description = $"[GetUsers] : {users.Count} elements found!";
                baseResponse.StatusCode = StatusCode.OK;
                foreach (var user in users)
                {
                    usersViewModel.Users.Add(new UserViewModel(user));
                }
                baseResponse.Data = usersViewModel;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<UsersViewModel>()
                {
                    Description = $"[GetUsers] : {ex.Message}",
                    StatusCode = StatusCode.SERVER_ERROR,
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteUserById(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                {
                    baseResponse.Description = $"[DeleteUserById] : User not found!";
                    baseResponse.StatusCode = StatusCode.USER_NOT_FOUND;
                    baseResponse.Data = false;
                    return baseResponse;
                }
                baseResponse.Data = await _userRepository.Delete(user);
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteUserById] : {ex.Message}",
                    StatusCode = StatusCode.SERVER_ERROR,
                    Data = false
                };
            }
        }
        public async Task<IBaseResponse<bool>> SetUserStatus(int id, UserStatus status)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                {
                    baseResponse.Description = $"[SetUserStatus] : User not found!";
                    baseResponse.StatusCode = StatusCode.USER_NOT_FOUND;
                    baseResponse.Data = false;
                    return baseResponse;
                }
                user.Status = status;
                user.Updated = DateTime.Now;
                if (baseResponse.Data = await _userRepository.Update(user))
                {
                    baseResponse.StatusCode = StatusCode.OK;
                    return baseResponse;
                }
                throw new Exception("User status set unsuccessfully!");
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[SetUserStatus] : {ex.Message}",
                    StatusCode = StatusCode.SERVER_ERROR,
                    Data = false
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteUsersById(List<int> ids) //fix
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {

                foreach (int id in ids)
                {
                    var user = await _userRepository.GetById(id);
                    if (user == null)
                    {
                        baseResponse.Description += $"[DeleteUserById] : User {id} not found!\n";
                    }
                    await _userRepository.Delete(user);
                }
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteUserById] : {ex.Message}",
                    StatusCode = StatusCode.SERVER_ERROR,
                    Data = false
                };
            }
        }

        public async Task<IBaseResponse<bool>> SetUsersStatusById(List<int> ids, UserStatus status)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                foreach (int id in ids)
                {
                    var user = await _userRepository.GetById(id);
                    if (user == null)
                    {
                        baseResponse.Description += $"[SetUserStatus] : User {id} not found!\n";
                    }
                    user.Status = status;
                    user.Updated = DateTime.Now;

                    if (!await _userRepository.Update(user))
                    {
                        baseResponse.Description += $"[SetUserStatus] : User {id} wasnt set successfully!\n";
                    }
                }
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[SetUserStatus] : {ex.Message}",
                    StatusCode = StatusCode.SERVER_ERROR,
                    Data = false
                };
            }
        }

        public async Task<IBaseResponse<UserViewModel>> GetUserByName(string name)
        {
            var baseResponse = new BaseResponse<UserViewModel>();
            try
            {
                var user = await _userRepository.GetByUserName(name);
                if (user == null)
                {
                    baseResponse.Description = $"[GetUserByName] : User not found!";
                    baseResponse.StatusCode = StatusCode.USER_NOT_FOUND;
                    return baseResponse;
                }
                baseResponse.Data = new UserViewModel(user);
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserViewModel>()
                {
                    Description = $"[GetUserByName] : {ex.Message}",
                    StatusCode = StatusCode.SERVER_ERROR,
                };
            }
        }

        public List<int> GetUserIds(UsersViewModel usersViewModel)
        {
            return usersViewModel.Users.Where(x => x.isSelected).Select(x => x.Id).ToList();
        }
    }
}
