using System.ComponentModel.DataAnnotations;

namespace Task4.Domain.Enums
{
    public enum UserStatus
    {
        [Display(Name="Активен")]
        Active=0,
        [Display(Name="Заблокирован")]
        Blocked = 1,
    }
}
