using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4.Domain.ViewModels.User
{
    public class UsersViewModel
    {
        public bool SelectAll { get; set; }
        public List<UserViewModel> Users { get; set; }
        public UsersViewModel()
        {
            Users = new List<UserViewModel>();
        }
    }
}
