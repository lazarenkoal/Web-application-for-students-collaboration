using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CocktionMVC.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Странно, что ваше имя превышает 25 символов...")]
        [Display(Name = "Имя по паспорту;)")]
        public string UserRealName { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Странно, что ваша фамилия превышает 25 символов...")]
        [Display(Name = "Фамилия по паспорту!!!")]
        public string UserRealSurname { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Название университета не должно превышать 100 символов")]
        [Display(Name = "Ваш университет")]
        public string University { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Название факультета не должно превышать 100 символов")]
        [Display(Name = "Ваш факультет")]
        public string Faculty { get; set; }

        [StringLength(100, ErrorMessage = "Слишком длинное название общежития")]
        [Display(Name = "Адрес общежития")]
        public string Dormitory { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Адрес учебного корпуса слишком длинный!")]
        [Display(Name = "Адрес учебного корпуса")]
        public string StudyAdress { get; set; }
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
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Странно, что ваше имя превышает 25 символов...")]
        [Display(Name = "Имя по паспорту;)")]
        public string UserRealName { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Странно, что ваша фамилия превышает 25 символов...")]
        [Display(Name = "Фамилия по паспорту!!!")]
        public string UserRealSurname { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пароль должен быть как минимум {2} значный.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

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
