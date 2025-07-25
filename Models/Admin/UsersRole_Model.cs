﻿using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.Admin
{ 
    public class UsersRole_Model
    {
        [Key]
        public string Id { get; set; }


        [Required(ErrorMessage = "Role name is required")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
