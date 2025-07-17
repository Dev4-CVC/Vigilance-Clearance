using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.PESB;

namespace VigilanceClearance.Models.Admin
{ 
    public class UsersMain_Model
    {
        public Users_Model users{ get; set; }
        public List<Users_Model> users_List { get; set; }
        public List<SelectListItem> roles_ddl_List { get; set; } = new List<SelectListItem>();


        public UsersRole_Model users_role { get; set; }
        public List<UsersRole_Model> users_role_List { get; set; }
    }
}
