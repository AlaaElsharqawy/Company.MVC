using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class ResetPasswordViewModel
    {


        [Required(ErrorMessage = "New Password Is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Confirm New Password  Is Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = " New Password Doesn't Match")]
        public string ConfirmNewPassword { get; set; }
    }
}
