using ImageMagick;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace application.Utils {
    
    public static class IFormFileExtension {
        public static async Task<byte[]> GetBytesAsync(this IFormFile file) {
            using (var stream = new MemoryStream()) {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                return stream.ToArray();
            }
        }
    }

    public static class ByteExtension {
        public static byte[] CompressBytes(this byte[] bytes) {
            using(var stream = new MemoryStream(bytes)) {
                var optimizer = new ImageOptimizer();
                optimizer.Compress(stream);

                return stream.ToArray();
            }
        }

        public static string GetBase64String (this byte[] bytes) {
            return Convert.ToBase64String(bytes);
        }
    }
    
}
