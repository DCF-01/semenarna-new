using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Areas.Panel.ViewModels {
    public class CategoryViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductCount { get; set; }
    }
}
