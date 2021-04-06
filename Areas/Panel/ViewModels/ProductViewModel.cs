using Microsoft.AspNetCore.Http;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.ViewModels {
    public class ProductViewModel {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Img { get; set; }
        public string GetImg { get; set; }
    }
}