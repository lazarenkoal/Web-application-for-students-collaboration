using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CocktionMVC.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage="Ну никак нельзя обойтись без почтового адреса!")]
        [EmailAddress(ErrorMessage="Введенный имейл недействителен!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage="Необходимо ввести свое имя!")]
        [StringLength(25, ErrorMessage = "Странно, что ваше имя превышает 25 символов...")]
        [Display(Name = "Ваше имя")]
        public string UserRealName { get; set; }

        [Required(ErrorMessage = "Необходимо ввести свою фамилию")]
        [StringLength(25, ErrorMessage = "Странно, что ваша фамилия превышает 25 символов...")]
        [Display(Name = "Ваша фамилия")]
        public string UserRealSurname { get; set; }

        [Required(ErrorMessage = "Необходимо ввести номер телефона!")]
        [RegularExpression("^[0-9\\-\\+]{11,12}$", ErrorMessage="Ваш номер телефона недействителен!")]
        public string PhoneNumber { get; set; }

    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Запомнить этот браузер?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ну никак нельзя обойтись без почтового адреса!")]
        [EmailAddress(ErrorMessage = "Введенный имейл не действителен!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо ввести свое имя!")]
        [StringLength(25, ErrorMessage = "Странно, что ваше имя превышает 25 символов...")]
        [Display(Name = "Ваше имя")]
        public string UserRealName { get; set; }

        [Required(ErrorMessage = "Необходимо ввести свою фамилию")]
        [StringLength(25, ErrorMessage = "Странно, что ваша фамилия превышает 25 символов...")]
        [Display(Name = "Ваша фамилия")]
        public string UserRealSurname { get; set; }

        [Required(ErrorMessage = "Необходимо ввести номер телефона!")]
        [RegularExpression("^[0-9\\-\\+]{11,12}$", ErrorMessage = "Ваш номер телефона недействителен!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Необходимо придумать пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен быть как минимум {2} значный.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage="Необходимо подтвердить пароль!")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Упс, пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пароль должен быть как минимум 6 значный", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Упс, пароли не совпадают.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
