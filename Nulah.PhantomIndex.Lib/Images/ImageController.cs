using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Nulah.PhantomIndex.Lib.Images
{
    public class ImageController
    {
        /// <summary>
        /// Resizes an image on disk and returns a byte[] of the result
        /// <para>
        /// This does not modify the original image
        /// </para>
        /// </summary>
        /// <param name="imageSource"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public async Task<byte[]> ResizeImageToWidth(string imageSource, int width)
        {
            using (var memStream = new MemoryStream())
            {
                using (var input = await Image.LoadAsync(imageSource))
                {
                    input.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = CalculateImageResize(width, input.Width, input.Height),
                        Mode = ResizeMode.Max
                    }));

                    await input.SaveAsync(memStream, new PngEncoder()
                    {
                        CompressionLevel = PngCompressionLevel.BestCompression
                    });
                }

                memStream.Position = 0;

                return memStream.ToArray();
            }
        }

        private Size CalculateImageResize(int targetWidth, int originalWidth, int originalHeight)
        {
            if (targetWidth <= originalWidth)
            {
                var ratio = targetWidth / (double)originalWidth;
                var ratioHeight = originalHeight * ratio;
                return new Size(targetWidth, (int)ratioHeight);
            }

            return new Size(originalWidth, originalHeight);
        }
    }
}
