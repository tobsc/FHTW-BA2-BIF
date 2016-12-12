using System.ComponentModel.DataAnnotations;

namespace HwInf.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Uid")]
        public string Uid { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}