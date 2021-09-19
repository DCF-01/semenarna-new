using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Areas.Store.ViewModels {
    public class StoreSpecViewModel {
        public string Name { get; set; }
        public string[] First { get; set; }
        public string[] Rest { get; set; }
        public int ItemsPerRow { get; set; }



    }
}