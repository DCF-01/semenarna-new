using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.ViewModels {
    public class UserViewModel {
        public List<IdentityUser> Users { get; set; }

    }
}
