using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.Modal_Properties.PESB
{
    public class PESB_Add_New_Reference_Model
    {
        public string? referenceReceivedFor { get; set; }
        public string? referenceReceivedFrom { get; set; }
        public string? referenceReceivedFromCode { get; set; }

        public string? selectionForThePostCategory { get; set; }
        public string? selectionForThePostSubCategory { get; set; }

        public string? orgCode { get; set; }
        public string? orgName { get; set; }
        public string? minCode { get; set; }
        public string? minDesc { get; set; }

        public string? referenceNoFileNo { get; set; } = null;
        public string? referenceOrSubmissionToCvcDate { get; set; }
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
