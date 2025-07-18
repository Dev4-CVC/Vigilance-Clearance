using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel.PESB
{
    public class officer_posting_details_model
    {
        //public int OfficerId { get; set; }
        //public int referenceid { get; set; }
        public int Id  { get; set; }
        public int vcOfficerId  { get; set; }

        public string orgCode { get; set; }
        public string orgMinistry { get; set; }
        public string designation { get; set; }
        public string placeOfPosting { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "From Date")]
        public DateTime fromDate { get; set; }

      
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "To Date")]
        public DateTime toDate { get; set; }


        public string actionBy { get; set; }
        public string actionBy_SessionId { get; set; }
        public string actionBy_IP { get; set; }


    }
}
