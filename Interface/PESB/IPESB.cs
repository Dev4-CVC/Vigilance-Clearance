﻿using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.Modal_Properties.PESB;
using VigilanceClearance.Models.PESB;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Interface.PESB
{
    public interface IPESB
    {
        //Action Method: PESB_Add_New_Reference
        Task<List<SelectListItem>> GetReferenceDropDownAsync();
        Task<List<SelectListItem>> GetPostDescriptionsDropDownAsync(string ReferenceCode);
        Task<List<SelectListItem>> GetSubPostsByPostCodeInternal(string postCode);
        Task<List<SelectListItem>> GetOrganizationDropDownAsync(string id);
        Task<List<SelectListItem>> GetMinistryByPostCodeInternal(string id);
        Task<int> InsertAddNewReferenceAsync(PESB_Add_New_Reference_Model objmodel);
        Task<List<VcReferenceReceivedFor_VM>> Get_VC_ReferenceReceivedFor_List_GetByIdAsync(int id, string username);
        Task<List<VcReferenceReceivedFor_VM>> Get_VC_ReferenceReceivedFor_Details_Async(int id);
        Task<int> InsertOfficerDetailsAsync(OfficerDetails_Model objmodel);
        Task<int> InsertOfficerPostingDetailsAsync(OfficerPostingDetails_Model objmodel);

        //new operation:  dated on 9 july 2025
        //new reference
        Task<int> Insert_Add_New_Reference_Async(new_reference_model objmodel);
        Task<List<new_reference_model>> Get_Vc_Reference_Received_For_List_GetById_and_Username_Async(int id, string username);
        Task<List<new_reference_model>> Get_Vc_Reference_Received_For_Details_GetById_Async(int id);

        //officer details and officer posting details
        Task<int> Insert_Officer_Details_Async(officer_details_model objmodel);
        Task<int> Insert_Officer_Posting_Details_Async(officer_posting_details_model objmodel);
        Task<List<SelectListItem>> Get_Service_DropDownAsync();
        Task<List<SelectListItem>> Get_Batch_DropDownAsync();
        Task<List<SelectListItem>> Get_Cadre_DropDownAsync();
        Task<List<officer_details_model>> Get_Officer_List_GetById_Async(int id);
        Task<List<officer_posting_details_model>> Get_Officer_Posting_List_GetById_Async(int id);
        Task<List<officer_details_model>> Get_Officer_Details_Async(int id);
        Task<List<officer_posting_details_model>> Get_Officer_Details_GetbyIdAsync(int id);
        Task<int> Update_Reference_From_PESB_Async(PESB_Update_Referencefrom_model objmodel);
    }
}