using System.ComponentModel.DataAnnotations;

namespace ClientManagerApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Логин обязателен для заполнения")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email обязателен для заполнения")]
        [EmailAddress(ErrorMessage = "Неверный адрес электронной почты")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен для заполнения")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Compare("Password",ErrorMessage ="Пароли не совпадают")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Address { get; set; }
    }
}
