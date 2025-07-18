using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.Admin
{
    public class UserRole_Model
    {
        [Key]
        public string Id { get; set; }


        [Required(ErrorMessage = "Role name is required")]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
