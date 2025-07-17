using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.Admin;
using VigilanceClearance.Models.PESB;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Interface.Admin
{
    public interface IAdmin
    {
        Task<List<Users_Model>> Get_Users_List_Async();
        Task<List<SelectListItem>> Get_Roles_Dropdown_Async();
        Task<int> Insert_Add_New_User_Async(Users_Model objmodel);


        Task<List<UsersRole_Model>> Get_Users_Role_List_Async();
        Task<int> Insert_Add_New_User_Role_Async(UsersRole_Model objmodel);



       
    }
}
