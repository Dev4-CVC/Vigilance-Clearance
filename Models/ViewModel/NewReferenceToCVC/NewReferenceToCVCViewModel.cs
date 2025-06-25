using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel.NewReferenceToCVC
{
    public class NewReferenceToCVCViewModel
    {
        [Required(ErrorMessage = "Please select Empanelment")]
        public string? Empanelment { get; set; }
        public List<SelectListItem> EmpanelmentList { get; set; } = new();


        [Required(ErrorMessage = "Please select Appointment")]
        public string? Appointment { get; set; }
        public List<SelectListItem> AppointmentList { get; set; } = new();


        [Required(ErrorMessage = "Please Enter Other Extension")]
        public string? Extension { get; set; }
        public List<SelectListItem> ExtensionList { get; set; } = new();
    }
}
