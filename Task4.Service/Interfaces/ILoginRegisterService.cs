using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Task4.Domain.Response;
using Task4.Domain.ViewModels.LoginRegister;

namespace Task4.Service.Interfaces
{
    public interface ILoginRegisterService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);
        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
    }
}
