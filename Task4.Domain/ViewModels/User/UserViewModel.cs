using Task4.Domain.Enums;

namespace Task4.Domain.ViewModels.User
{
    public class UserViewModel
    {
        public UserViewModel(Task4.Domain.Models.User user)
        {
            Id=user.Id;
            UserName=user.UserName;
            Email=user.Email;
            Status=user.Status;
            Created=user.Created;
            Updated=user.Updated;
        }
        public UserViewModel()
        {
            
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; }
        public bool isSelected { get; set; }
    }
}
