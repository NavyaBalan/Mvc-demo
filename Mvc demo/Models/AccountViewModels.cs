using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_demo.Models
{
    public class AccountViewModels
    {

        public class LoginViewModels
        {
            [Required]
            [Display(Name = "Email")]
            [EmailAddress]
            public string Email { get; set; }
            [Display(Name = "UserName")]
            [Required]
            public string UserName { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]

            public string Password { get; set; }

        }
        public class RegisterViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "UserName")]
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [StringLength(100,ErrorMessage = "The {0} must be atleast {2} characters long",MinimumLength  = 6)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [Compare("Password", ErrorMessage = "The password and ConfirmPassword do not match ")]
            public string ConfirmPassword { get; set; }


        }

        public class ResetPasswordViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [StringLength(100, ErrorMessage = "The {0} must be atleast {2} characters long", MinimumLength = 6)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [Compare("Password", ErrorMessage = "The password and ConfirmPassword do not match ")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public class ForgotPasswordViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }
    }
}