﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Models {
    public class Spec {
        public int SpecId { get; set; }
        public string Name { get; set; }
        public string[] First { get; set; }
        public string[] Second { get; set; }
        public string[] Third { get; set; }
        public string[] Fourth { get; set; }
    }
}
