using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel.PESB
{
    public class VC_Post_ViewModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "PostCode is required.")]
        public string PostCode { get; set; }


        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
    }
}
