using System.ComponentModel.DataAnnotations;

namespace ClientManagerApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Требуется имя пользователя.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Требуется пароль.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name ="Запомнить меня.")]
        public bool RememberMe { get; set; }
    }
}
