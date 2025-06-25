using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel.Account
{
    public class LoginViewModel
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }


        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DisplayName("Captcha Code")]
        [Required(ErrorMessage = "Captcha is required.")]
        public string Captcha { get; set; }
    }
}
