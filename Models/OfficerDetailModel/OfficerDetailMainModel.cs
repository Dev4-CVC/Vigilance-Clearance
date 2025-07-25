using VigilanceClearance.Models.Dealing_Hand;
using VigilanceClearance.Models.Modal_Properties;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerDetailMainModel
    {
        public OfficerPersonalDetailModel officerPersonalDetailModel { get; set; }

        public OfficerPostingDetails officerPostingDetail7 { get; set; }

        public OfficerIntegrityAgreedOrDoubtful officerIntegrityAgreedOrDoubtful_8 { get; set; }

        public OfficerAllegationOfMisconductExamined officerAllegationOfMisconductExamined_9 { get; set; }

        public OfficerPunishmentAwarded officerPunishmentAwarded_10 { get; set; }

        public OfficerDisciplinaryCriminalProceedings officerDisciplinaryCriminalProceedings_11 { get; set; }

        public OfficerActionContemplatedAgainstTheOfficerAsOnDate officerActionContemplatedAgainstTheOfficerAsOnDate_12 { get; set; }
        public OfficerComplaintWithVigilanceAnglePending officerComplaintWithVigilanceAnglePending_13 { get; set; }

        public List<OfficerPostingDetailsViewModellist> officerPostingDetailsList { get; set; }
        public List<InsertIntegrityAgreedOrDoubtfulModel> insertIntegrityAgreedOrDoubtfulModellist { get; set; }
        public List<AllegationOfMisconductExaminedModel> AllegationOfMisconductExaminedModellist { get; set; }
        public List<PunishmentAwardedModel> PunishmentAwardedModellist { get; set; }
        public List<DisciplinaryCriminalProceedingsModel> DisciplinaryCriminalProceedingsModellist { get; set; }
        public List<ActionContemplatedAgainstTheOfficerModel> ActionContemplatedAgainstTheOfficerModellist { get; set; }
        public List<ComplaintWithVigilanceAnglePendingModel> ComplaintWithVigilanceAnglePendingModellist { get; set; }

        public PESB.new_reference_model new_Reference_Model { get; set; }



        #region added by Sudarshan 23 7 2025
        public VigBranchCommentsInsert vig_branch_comments{ get; set; }

        #endregion

    }
}
