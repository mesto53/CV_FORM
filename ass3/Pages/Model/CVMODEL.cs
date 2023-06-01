    using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace ass3.Pages.Model
{
    public class CVMODEL
    {
        [Required]
        [Display(Name = "UPLOAD PHOTO    :   ")]
        public IFormFile Photo { get; set; }

        public List<SelectListItem> CountryList { get; set; } = new List<SelectListItem>();
        [Required]
        [Display(Name = "SKILLS   ")]
        public Dictionary<string, bool> SkillsDictionary { get; set; } = new Dictionary<string, bool>()
        {
            { "JAVA", true },
            { "PYTHON", false },
            { "ASP.CORE", false }
        };

        [Required]
        [Display(Name = "VERFICATION")]
        public Verf Verfication { get; set; } = new Verf();

        [Required]
        [Display(Name = "CONFIRM PASSWORD : ")]
        [PasswordPropertyText]
        public string ConfirmPass { get; set; } = "";
        public class Country
        {
            public string Name { get; set; } = "";
        }
        public class Verf
        {
            private static readonly Random random = new Random();

            public int Nb1 { get; set; } = random.Next(1, 100);

            public int Nb2 { get; set; } = random.Next(1, 100);
            public int Nb3 { get; set; } = 0;


        }
    }
}
