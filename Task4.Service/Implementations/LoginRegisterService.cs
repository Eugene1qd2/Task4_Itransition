using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Task4.DAL.Interfaces;
using Task4.Domain.Enums;
using Task4.Domain.Models;
using Task4.Domain.Response;
using Task4.Domain.ViewModels.LoginRegister;
using Task4.Service.Interfaces;

namespace Task4.Service.Implementations
{
    public class LoginRegisterService : ILoginRegisterService
    {
        private readonly IUserRepository _userRepository;

        public LoginRegisterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = (await _userRepository.GetAll()).FirstOrDefault(x => x.UserName == model.UserName);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "User not found!"
                    };
                }
                if (user.Password != HashPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Wrong Login or Password!"
                    };
                }
                if (user.Status == UserStatus.Blocked)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "User blocked!"
                    };
                }

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.SERVER_ERROR
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
        {
            try
            {
                var user = (await _userRepository.GetAll()).FirstOrDefault(x => x.UserName == model.UserName);
                if (user != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "User with such UserName already exists",
                    };
                }

                user = new User()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = HashPassword(model.Password),
                    Status = UserStatus.Active,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                };

                await _userRepository.Create(user);

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "User created",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.SERVER_ERROR
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Status.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedPass = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedPass).Replace("-", "").ToLower();

                return hash;
            }
        }
    }
}
