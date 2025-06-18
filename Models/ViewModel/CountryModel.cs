using System.Xml.Linq;

namespace VigilanceClearance.Models.ViewModel
{
    public class CountryModel
    {
        public Name Name { get; set; }
        public string Cca2 { get; set; }
    }

    public class Name
    {
        public string Common { get; set; }
    }
}
