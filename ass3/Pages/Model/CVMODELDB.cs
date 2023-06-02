using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ass3.Pages.Model
{
    public class CVMODELDB
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "FIRST NAME" )]
        public string FirstName { get; set; } = "";

        [Required]
        [Display(Name = "Last NAME")]
        public string LastName { get; set; } = "";


        [Required]
        [Display(Name = "BIRTHDAY")]
        public DateTime BDay { get; set; } = new DateTime(2000, 7, 3, 12, 30, 0);


        [Required]
        [Display(Name = "Nationality")]
        
        public string  Nationality { get; set; } = "";


        [Required]
        [Display(Name = "SEX  ")]
        public string  Sex { get; set; } = "Male";


        [Required]
        [Display(Name = "EMAIL  ")]
        [EmailAddress]
        public string  Email { get; set; } = "";


        [Required]
        [Display(Name = "PASSWORD ")]
        [PasswordPropertyText]
        public string Pass { get; set; } = "";

        public string Photo { get; set; } = "";

        public string Skills { get; set; } = "";
    }

}
