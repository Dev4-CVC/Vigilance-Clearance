using Microsoft.AspNetCore.Mvc.Rendering;

namespace VigilanceClearance.Models.ViewModel
{
    public class CountryViewModel
    {
        public string SelectedCountry { get; set; }

        public List<SelectListItem> CountryList { get; set; }
    }
}
