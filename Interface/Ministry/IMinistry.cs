using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.Ministry_ApproverModels;
using VigilanceClearance.Models.Modal_Properties;
using VigilanceClearance.Models.Modal_Properties.PESB;
using VigilanceClearance.Models.New_Reference_to_CVCModels;
using VigilanceClearance.Models.OfficerDetailModel;
using VigilanceClearance.Models.PESB;
using VigilanceClearance.Models.ViewModel.Ministry;

namespace VigilanceClearance.Interface.Ministry
{
    public interface IMinistry
    {
        Task<List<SelectListItem>> GetOrganizationDropDownAsync(string section);
        Task<List<SelectListItem>> GetMinistryDropDownbycodeAsync(string orgcode);
        Task<int> InsertOfficerPostingDetail(InsertOfficerDetailsModel objmodel);

        #region Added As on date 08_07_2025
        Task<List<ReferenceReceivedFromCVCModel>> GetReferenceReceivedFromCVClist(string _UserName);

        #endregion


        // Added code as on date 02_07_2025 
        Task<List<OfficerListModel>> GetOfficerListAsync(string id);

        Task<List<OfficerPostingDetailsViewModellist>> GetOfficerPostingList(string id);

        // Added code as on date 02_07_2025 


        #region Added as on date 03_07_2025

        Task<int> InsertIntegrityAgreedOrDoubtful(InsertIntegrityAgreedOrDoubtfulModel _insertIntegritymodel);

        Task<List<InsertIntegrityAgreedOrDoubtfulModel>> GetInsertIntegrityAgreedOrDoubtfulList(string id);

        #endregion

        #region Added as on date 04_07_2025
        //9
        Task<List<AllegationOfMisconductExaminedModel>> GetAllegationOfMisconductExaminedList(string id);
        Task<int> InsertAllegationOfMisconductExamined(AllegationOfMisconductExaminedModel _insertAllegationOfMisconduct);

        //10
        Task<int> InPunishmentAwarded(PunishmentAwardedModel _punishmentAwarded);
        Task<List<PunishmentAwardedModel>> GetPunishmentAwardedList(string id);

        #endregion

        //11 

        Task<int> InsertDisciplinaryCriminalProceedings(DisciplinaryCriminalProceedingsModel _disciplinaryCriminalProceedings);
        Task<List<DisciplinaryCriminalProceedingsModel>> GetDisciplinaryCriminalProceedingsModelList(string id);

        //12

        Task<int> InsertActionContemplatedAgainstTheOfficer(ActionContemplatedAgainstTheOfficerModel _actionContemplatedAgainstTheOfficerModel);
        Task<List<ActionContemplatedAgainstTheOfficerModel>> GetActionContemplatedAgainstTheOfficerlList(string id);

        //13

        Task<int> InsertComplaintWithVigilanceAnglePending(ComplaintWithVigilanceAnglePendingModel _vigilanceAnglePendingModel);
        Task<List<ComplaintWithVigilanceAnglePendingModel>> GetComplaintWithVigilanceAnglePendingList(string id);


        // Start the code for the New Reference To CVC

        Task<int> Insert_New_Reference(InsertReferenceReceivedForModel _insertRefMode);

        // End the code for the New Reference To CVC

        #region Added as on date 14_07_2025
        Task<int> Insert_New_Officer_Details(InsertNewOfficerDetailFromMinistryModel insertNewOfficerDetailFromMinistryModel);

        #endregion

        #region Added as on date 16_07_2025

        Task<int> UpdateReferenceFromPESB(UpdateRefFromPESBModel _updateRefModel);


        Task<List<OfficerListModel>> GetOfficerDetailsByOfficerIdAsync(string id);

        #endregion


        #region Added as on date 22_07_2025 
        Task<List<ReferenceListPendingWith_MinistryApproverModel>> GetReferenceListPendingWith_MinistryApproverList(string _MinCode, string _Role);
        #endregion


        #region Added as on date 24_07_2025 For the References_from_coord2
        Task<List<References_from_coord2_To_MinistryModel>> References_from_coord2_To_Ministry(string _Mincode);
        #endregion 

    }
}
