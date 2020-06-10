using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.ViewModels
{
    public class ChangePasswordViewModel
    {
        public int id { get; set; }
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле", AllowEmptyStrings = false)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
