using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel
{
    public class PESBViewModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please select a Post")]
        public string? PostCode { get; set; }
        public List<SelectListItem> PostList { get; set; }

    }
}
