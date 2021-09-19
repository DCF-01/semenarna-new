using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Areas.Cart.ViewModels {
    public class CartViewModel {
        public CartProductViewModel[] Items { get; set; }
        public string UserId { get; set; }
    }
}
