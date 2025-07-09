using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel.PESB
{
    public class VcReferenceReceivedFor_VM                              //for showing table on appointment page
    {
        public List<VcReferenceReceivedFor_VM> data_List { get; set; }

        public int Id { get; set; }

        [Display(Name = "Reference Received For")]
        public string? referenceReceivedFor { get; set; }

        [Display(Name = "Reference Received From")]
        public string? referenceReceivedFrom { get; set; }

        [Display(Name = "Reference Received From Code")]
        public string? referenceReceivedFromCode { get; set; }

        [Display(Name = "Post")]
        public string? selectionForThePostCategory { get; set; }

        [Display(Name = "Sub Post")]
        public string? selectionForThePostSubCategory { get; set; }

        [Display(Name = "Organization Name")]
        public string? orgCode { get; set; }
        public string? orgName { get; set; }

        [Display(Name = "Ministry Name")]
        public string? minCode { get; set; }
        public string? minDesc { get; set; }

        [Display(Name = "Reference File No")]
        public string? ReferenceNo_FileNo { get; set; } = null;

        [Display(Name = "Reference Submission Date")]
        public string? referenceOrSubmissionToCvcDate { get; set; }

        [Display(Name = "Reference Id File No")]
        public string? CVC_ReferenceID_FileNo { get; set; }

        [Display(Name = "User")]
        public string? createdBy { get; set; }
        public DateTime? createdOn { get; set; }
        public string? createdBySessionId { get; set; }
        public string? createdByIp { get; set; }


        public string? updateBy { get; set; }
        public DateTime? updatedOn { get; set; }
        public string? updatedBySessionId { get; set; }
        public string? updatedByIp { get; set; }

        [Display(Name = "Pending With")]
        public string? pendingWith { get; set; }

        [Display(Name = "UID")]
        public string? uid { get; set; }

        [Display(Name = "Reference Id")]
        public string? referenceId { get; set; }

    }
}
