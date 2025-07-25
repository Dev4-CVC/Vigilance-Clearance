using System.Text.Json.Serialization;

namespace VigilanceClearance.Models.ViewModel.Coord2_DealingHand
{
    public class OfficerDetailsWithPostings
    {
        [JsonPropertyName("MasterReferenceID")]
        public int? MasterReferenceID { get; set; }

        [JsonPropertyName("Id")]
        public int? Id { get; set; }

        [JsonPropertyName("Officer_Name")]
        public string? Officer_Name { get; set; }

        [JsonPropertyName("Officer_FatherName")]
        public string? Officer_FatherName { get; set; }

        [JsonPropertyName("Officer_DateOfBirth")]
        public DateTime? Officer_DateOfBirth { get; set; }

        [JsonPropertyName("Officer_RetirementDate")]
        public DateTime? Officer_RetirementDate { get; set; }

        [JsonPropertyName("Officer_ServiceEntryDate")]
        public DateTime? Officer_ServiceEntryDate { get; set; }

        [JsonPropertyName("Officer_Batch_Year")]
        public string Officer_Batch_Year { get; set; }

        [JsonPropertyName("Officer_Cadre")]
        public string Officer_Cadre { get; set; }

        [JsonPropertyName("id")]
        public int? id { get; set; }

        [JsonPropertyName("Vc_Officer_Id")]
        public int? Vc_Officer_Id { get; set; }

        [JsonPropertyName("orgCode")]
        public string orgCode { get; set; }

        [JsonPropertyName("designation")]
        public string designation { get; set; }

        [JsonPropertyName("placeOfPosting")]
        public string placeOfPosting { get; set; }

        [JsonPropertyName("orgMinistry")]
        public string orgMinistry { get; set; }

        [JsonPropertyName("cbi_comments")]
        public string? FeebbackOf_CBI { get; set; }

        [JsonPropertyName("vig_Branch_Comments")]
        public string? vig_Branch_Comments { get; set; }

        [JsonPropertyName("ministry_Comments")]
        public string? ministry_Comments { get; set; }

        [JsonPropertyName("CVC_COORD2_DH_COMMENTS")]
        public string? CVC_COORD2_DH_COMMENTS { get; set; }

        [JsonPropertyName("CVC_COORD2_SO_COMMENTS")]
        public string? CVC_COORD2_SO_COMMENTS { get; set; }

        [JsonPropertyName("CVC_COORD2_BO_COMMENTS")]
        public string? CVC_COORD2_BO_COMMENTS { get; set; }

        public int? masterRefereceIdComments {  get; set; }  
        public int? OfficerIdComments {  get; set; }  
    }
}
