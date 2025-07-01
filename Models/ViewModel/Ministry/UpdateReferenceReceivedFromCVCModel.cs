using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel.Ministry
{
    public class UpdateReferenceReceivedFromCVCModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Name of the Officer")]
        public string? Officer_Name { get; set; }
        [Display(Name = "Father's Name")]
        public string? Officer_FatherName { get; set; }
        [Display(Name = "Date of Birth")]
        public string? Officer_DateOfBirth { get; set; }
        [Display(Name = "Date Of Retirement")]
        public string? Officer_RetirementDate { get; set; }
        [Display(Name = "Date of Entry into Service")]
        public string? Officer_ServiceEntryDate { get; set; }
        [Display(Name = "Service")]
        public string? Officer_Service { get; set; }
        [Display(Name = "Batch")]
        public string? Officer_Batch_Year { get; set; }

        public string? Cadre { get; set; }


        public string? SelectedCountry { get; set; }

        public List<SelectListItem>? CountryList { get; set; }

//7 PostionHeld
        public string? Organization { get; set; }
        public List<SelectListItem> OrganizationList { get; set; } = new();
        public string? Ministry { get; set; }
        public List<SelectListItem> MinistryList { get; set; } = new();
        public string? Designation { get; set; }
        public DateTime TenureFrom { get; set; }
        public DateTime TenureTo { get; set; }

//8 

        public bool? IsAgreed { get; set; }
        public DateTime YearFrom { get; set; }
        public DateTime YearTo { get; set; }
        public DateTime RemovedFromAgreedlistDate  { get; set; }


        //9
        public string? AllegationMisconduct { get; set; }
        public string? Casedetails { get; set; }
        public DateTime Presentstatusasondate { get; set; }


        //10

        public string? PunishmentAwarded { get; set; }
        public string? Penaltydetails { get; set; }
        public DateTime CurrencyofpenaltyFrom { get; set; }
        public DateTime CurrencyofpenaltyTo { get; set; }

        //11 
        public string? DisciplinaryProceeding { get; set; }
        public string? SuspensionDate { get; set; }
        public string? WhetherRevoked { get; set; }
        public DateTime DateofRevocation { get; set; }

        //12

        public string? ContemplatedAgainst { get; set; }
        public string? ContemplatedAgainstCaseDetail { get; set; }
        public string? ContemplatedAgainstCasestatus { get; set; }
        public DateTime ContemplatedAgainstStatusasondate { get; set; }

        //13

        public string? ComplaintwithVigilanceAngle  { get; set; }
        public string? ComplaintwithVigilanceAngleCaseDetail { get; set; }
        public string? ComplaintwithVigilanceAnglePresentstatus { get; set; }
        public DateTime ComplaintwithVigilanceAnglePresentstatusasondate { get; set; }


    }
}
