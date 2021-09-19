using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.ViewModels {
    public class RegisterViewModel {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsValid { get; }
    }
}
