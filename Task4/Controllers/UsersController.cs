using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Task4.DAL.Interfaces;
using Task4.Domain.Response;
using Task4.Domain.ViewModels.User;
using Task4.Service.Interfaces;

namespace Task4.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            if (await AuthenticateUser())
            {
                var response = await _userService.GetUsers();
                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                    return View(response.Data);
                return RedirectToAction("Error", "Home", response.StatusCode);
            }
            LogoutUser();
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers(UsersViewModel usersViewModel)
        {
            if (await AuthenticateUser())
            {
                var response = await _userService.DeleteUsersById(_userService.GetUserIds(usersViewModel));
                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                    return RedirectToAction("GetAllUsers");
                return RedirectToAction("Error", "Home", response.StatusCode);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> BlockUsers(UsersViewModel usersViewModel)
        {
            if (await AuthenticateUser())
            {
                List<int> ids = usersViewModel.Users.Where(x => x.isSelected).Select(x => x.Id).ToList();
                var response = await _userService.SetUsersStatusById(_userService.GetUserIds(usersViewModel), Domain.Enums.UserStatus.Blocked);
                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                    return RedirectToAction("GetAllUsers");
                return RedirectToAction("Error", "Home", response.StatusCode);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUsers(UsersViewModel usersViewModel)
        {
            if (await AuthenticateUser())
            {
                List<int> ids = usersViewModel.Users.Where(x => x.isSelected).Select(x => x.Id).ToList();
                var response = await _userService.SetUsersStatusById(_userService.GetUserIds(usersViewModel), Domain.Enums.UserStatus.Active);
                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                    return RedirectToAction("GetAllUsers");
                return RedirectToAction("Error", "Home", response.StatusCode);
            }
            return RedirectToAction("Error", "Home");
        }

        private async Task<bool> AuthenticateUser()
        {
            var response = await _userService.VerifyUser(User.Identity.Name);
            return User.Identity.IsAuthenticated && response.Data;
        }

        private async void LogoutUser()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
