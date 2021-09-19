using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace application.ViewModels {
    public class ResetPasswordViewModel {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The passwords don't match")]
        public string NewPasswordConfirm { get; set; }
        public string Message { get; set; }
        public string ResetToken { get; set; }
        public string UserId { get; set; }
    }
}
