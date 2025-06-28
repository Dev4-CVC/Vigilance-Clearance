using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VigilanceClearance.Models.ViewModel.PESB
{
    public class AddNewReference_VM
    {
        public string referenceReceivedFor { get; set; } = string.Empty;

        [ValidateNever]
        public List<SelectListItem> referenceReceivedFor_List { get; set; }



        [Required(ErrorMessage = "Please select Post")]
        public string selectionForThePostCategory { get; set; } = string.Empty;

        [ValidateNever]
        public List<SelectListItem> selectionForThePostCategory_List { get; set; }


        public string selectionForThePostSubCategory { get; set; } = string.Empty;

        [ValidateNever]
        public List<SelectListItem> selectionForThePostSubCategory_List { get; set; }



        [Required(ErrorMessage = "Please select Organization")]
        public string orgCode { get; set; } = string.Empty;

        [ValidateNever]
        public List<SelectListItem> orgName_List { get; set; }



        [Required(ErrorMessage = "Please select Ministry")]
        public string minCode { get; set; } = string.Empty;

        [ValidateNever]
        public List<SelectListItem> minDesc_List { get; set; }


        [Required(ErrorMessage = "Reference/File number is required.")]
        public string referenceNoFileNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Submission to CVC date is required.")]
        [RegularExpression(@"\d{4}-\d{2}-\d{2}", ErrorMessage = "Date must be in yyyy-mm-dd format")]
        public string referenceOrSubmissionToCvcDate { get; set; } = string.Empty;

        [Required(ErrorMessage = "CVC Reference ID/File number is required.")]
        public string cvcReferenceIdFileNo { get; set; } = string.Empty;

    }
}
