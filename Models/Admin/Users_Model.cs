using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VigilanceClearance.Models.Admin
{
    public class Users_Model
    {
        public string userId { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [Display(Name = "Roles")]
        [JsonPropertyName("selectedRole")]
        public string role { get; set; }


        [JsonPropertyName("roles")]
        public List<string> role_list { get; set; }
    }
}
