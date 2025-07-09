namespace VigilanceClearance.Models.Modal_Properties.PESB
{
    public class OfficerDetails_Model
    {
        public long masterReferenceID { get; set; }

        public string officer_Name { get; set; }
        public string officer_FatherName { get; set; }
        public DateTime officer_DateOfBirth { get; set; }
        public DateTime officer_ServiceEntryDate { get; set; }
        public DateTime officer_RetirementDate { get; set; }      
        public string officer_Service { get; set; }
        public string officer_other_Service { get; set; }
        public int officer_Batch_Year { get; set; }
        public string officer_Cadre { get; set; }


        public string createdBy { get; set; }
        public string createdBy_SessionId { get; set; }
        public string createdBy_IP { get; set; }
    }



    //public class OfficerDetails_Model
    //{


    //    public long masterReferenceID { get; set; }
    //    public string selectionForThePostCategory { get; set; }

    //    public string selectionForThePostSubCategory { get; set; }

    //    public string referenceReceivedFor { get; set; }

    //    public string othersRemarks { get; set; }

    //    public string orgCode { get; set; }


    //    //---------------------------------------------------
    //    public string officer_Name { get; set; }

    //    public string officer_FatherName { get; set; }

    //    public DateTime officer_DateOfBirth { get; set; }

    //    public DateTime officer_RetirementDate { get; set; }

    //    public DateTime officer_ServiceEntryDate { get; set; }

    //    public string officer_Service { get; set; }

    //    public string officer_Batch_Year { get; set; }

    //    public string officer_Cadre { get; set; }
    //    //---------------------------------------------------


    //    public string officerPostingDetails_7 { get; set; }

    //    public string integrityAgreedOrDoubtful_8 { get; set; }

    //    public string allegationOfMisconductExamined_9 { get; set; }

    //    public string punishmentAwarded_10 { get; set; }

    //    public string disciplinaryCriminalProceedings_11 { get; set; }

    //    public string actionContemplatedAgainstTheOfficerAsOnDate_12 { get; set; }

    //    public string complaintWithVigilanceAnglePending_13 { get; set; }

    //    public string createdBy { get; set; }

    //    public DateTime createdOn { get; set; }

    //    public string createdBy_SessionId { get; set; }

    //    public string createdBy_IP { get; set; }

    //    public string updateBy { get; set; }

    //    public DateTime updatedOn { get; set; }

    //    public string updatedBy_SessionId { get; set; }

    //    public string updatedBy_IP { get; set; }

    //    public string pendingWith { get; set; }

    //    public string uid { get; set; }
    //}


}
