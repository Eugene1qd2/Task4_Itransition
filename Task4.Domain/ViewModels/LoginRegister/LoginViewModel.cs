using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4.Domain.ViewModels.LoginRegister
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Input Username!")]
        [MaxLength(25, ErrorMessage = "Username should have less than 25 characters!")]
        [MinLength(3, ErrorMessage = "Username should contain at least 3 characters!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Input Password!")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
