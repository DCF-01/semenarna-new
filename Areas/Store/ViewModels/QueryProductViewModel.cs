using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Store.ViewModels {
    public class QueryProductViewModel {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }

        public byte[] UncompressedImg { get; set; }
        public string Img { get; set; }
    }
}
