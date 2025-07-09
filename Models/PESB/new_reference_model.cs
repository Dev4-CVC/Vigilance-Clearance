using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.PESB
{
    public class new_reference_model
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Reference Received For is required")]
        [Display(Name = "Reference")]
        public string? referenceReceivedFor { get; set; }
        public string? referenceReceivedFrom { get; set; }
        public string? referenceReceivedFromCode { get; set; }

        [Required(ErrorMessage = "Post is required")]
        [Display(Name = "Post")]
        public string? selectionForThePostCategory { get; set; }

        [Display(Name = "Sub Post")]
        public string? selectionForThePostSubCategory { get; set; }


        [Display(Name = "Organization")]
        public string? orgCode { get; set; }
        public string? orgName { get; set; }

        [Display(Name = "Ministry")]
        public string? minCode { get; set; }
        public string? minDesc { get; set; }


        [Display(Name = "Reference No./ File No.")]
        public string? referenceNoFileNo { get; set; } = null;
        
        [Display(Name = "Submission to CVC Date")]
        public string? referenceOrSubmissionToCvcDate { get; set; }

        [Display(Name = "Reference File No.")]
        public string? cvcReferenceIdFileNo { get; set; }


        public string? createdBy { get; set; }
        public DateTime? createdOn { get; set; }
        public string? createdBySessionId { get; set; }
        public string? createdByIp { get; set; }


        public string? updateBy { get; set; }
        public DateTime? updatedOn { get; set; }
        public string? updatedBySessionId { get; set; }
        public string? updatedByIp { get; set; }


        public string? pendingWith { get; set; }
        public string? uid { get; set; }
        public string? referenceId { get; set; }
    }
}
