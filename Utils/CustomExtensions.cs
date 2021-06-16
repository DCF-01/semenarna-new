using ImageMagick;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Utils {
    
    public static class IFormFileExtension {
        public static async Task<byte[]> GetBytesAsync(this IFormFile file) {
            using (var stream = new MemoryStream()) {
                var optimizer = new ImageOptimizer();
                await file.CopyToAsync(stream);
                stream.Position = 0;

                optimizer.Compress(stream);

                return stream.ToArray();


            }
        }
    }
    
}
