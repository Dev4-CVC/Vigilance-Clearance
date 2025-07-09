using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel.PESB
{
    public class OfficerPostingDetails_VM
    {
        public int OfficerId { get; set; }
        public int referenceid { get; set; }



        public string orgCode { get; set; }
        public string minCode { get; set; }
        public string Designation { get; set; }
        public string PlaceOfPosting { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "From Date")]
        public DateTime? FromDate { get; set; }

      
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "To Date")]
        public DateTime? ToDate { get; set; }


        [ValidateNever]
        public List<SelectListItem> orgName_List { get; set; }

        [ValidateNever]
        public List<SelectListItem> minDesc_List { get; set; }
    }
}
