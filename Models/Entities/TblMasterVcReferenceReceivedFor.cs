using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models
{
    public class TblMasterVcReferenceReceivedFor
    {
        public long Id { get; set; }

        public string? ReferenceReceivedFor { get; set; }

        public string? ReferenceReceivedFrom { get; set; }

        public string? ReferenceReceivedFromCode { get; set; }

        public string? SelectionForThePostCategory { get; set; }

        public string? SelectionForThePostSubCategory { get; set; }

        public string? OrgCode { get; set; }

        public string? OrgName { get; set; }

        public string? MinCode { get; set; }

        public string? MinDesc { get; set; }

        public string? ReferenceNoFileNo { get; set; }

        public DateTime? ReferenceOrSubmissionToCvcDate { get; set; }

        public string? CvcReferenceIdFileNo { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? CreatedBySessionId { get; set; }

        public string? CreatedByIp { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? UpdatedBySessionId { get; set; }

        public string? UpdatedByIp { get; set; }

        public string? PendingWith { get; set; }

        public Guid? Uid { get; set; }

        public string? ReferenceId { get; set; }
    }
}
