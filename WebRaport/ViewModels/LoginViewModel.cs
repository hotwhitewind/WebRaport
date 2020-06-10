using System.ComponentModel.DataAnnotations;

namespace WebRaport.ViewModels
{
    public class LoginViewModel
    {
        public string Login { set; get; }

        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { set; get; }

    }
}
