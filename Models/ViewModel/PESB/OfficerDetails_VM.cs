using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VigilanceClearance.Models.ViewModel.PESB
{
    public class OfficerDetails_VM
    {
        public int OfficerId { get; set; }
        public int referenceid { get; set; }


        [Required(ErrorMessage = "Please enter Officer Name")]
        [Display(Name = "Officer Name")]
        public string officer_Name { get; set; }


        [Required(ErrorMessage = "Please enter Father's Name")]
        [Display(Name = "Father's Name")]
        public string officer_FatherName { get; set; }


        [Required(ErrorMessage = "Please select DOB")]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime officer_DateOfBirth { get; set; }


        [Required(ErrorMessage = "Please select Date of Entry")]
        [Display(Name = "Date of Entry into Service")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime officer_ServiceEntryDate { get; set; }


        [Required(ErrorMessage = "Please select Date of Retirement")]
        [Display(Name = "Date of Retirement")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime officer_RetirementDate { get; set; }


        [Required(ErrorMessage = "Please select Service")]
        [Display(Name = "Service")]
        public string officer_Service { get; set; }

        [ValidateNever]
        public List<SelectListItem> Service_List { get; set; } = new();


        [Required(ErrorMessage = "Please select Batch")]
        [Display(Name = "Batch")]
        public int officer_Batch_Year { get; set; }

        [ValidateNever]
        public List<SelectListItem> Batch_List { get; set; } = new();


        [Display(Name = "Cadre")]
        public string officer_Cadre { get; set; }

        [ValidateNever]
        public List<SelectListItem> Cadre_List { get; set; } = new();


        [Display(Name = "Other Service")]
        public string OtherService { get; set; }








        //modal popup dropdown list
        [Required(ErrorMessage = "Please select an organization.")]
        public string orgCode { get; set; }
        
        [ValidateNever]
        public List<SelectListItem> orgName_List { get; set; } = new();


        public string minCode { get; set; }
        [ValidateNever]
        public List<SelectListItem> minDesc_List { get; set; } = new();


        [Required(ErrorMessage = "Please enter designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Please enter place of posting")]
        public string PlaceOfPosting { get; set; }

        [Required(ErrorMessage = "Please enter from date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "Please enter to date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ToDate { get; set; }
    }
}