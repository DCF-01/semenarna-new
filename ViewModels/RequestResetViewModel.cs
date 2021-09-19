using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.ViewModels {
    public class RequestResetViewModel {
        public string Email { get; set; }
        public string Message { get; set; }
        public string ResetUrl { get; set; }
    }
}
