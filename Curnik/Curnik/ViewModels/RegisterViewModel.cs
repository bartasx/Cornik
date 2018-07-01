using System.ComponentModel.DataAnnotations;

namespace Curnik.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Podaj nazwę użytkownika")]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Podaj Hasło")]
        [StringLength(100, ErrorMessage = "{0} musi mieć minimum {2} i maksimum {1} znaków.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "AccountDescription")]
        public string AccountDescription { get; set; }

        [Required(ErrorMessage = "Podaj email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}