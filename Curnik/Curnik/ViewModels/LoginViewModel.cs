using System.ComponentModel.DataAnnotations;

namespace Curnik.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Podaj Login")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Podaj haslo")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}