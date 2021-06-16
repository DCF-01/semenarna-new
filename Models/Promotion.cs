﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Models {
    public class Promotion {
        public int PromotionId { get; set; }
        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public byte[] Img { get; set; }
        public bool Active { get; set; }
    }
}
