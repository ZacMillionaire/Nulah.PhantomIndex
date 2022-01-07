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
        public readonly static string FileDialogSupportedImageFormats;

        static ImageController()
        {
            FileDialogSupportedImageFormats = GetSupportedFileDialogFilterString();
        }

        /// <summary>
        /// Returns true if the image file is a supported image format
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public static bool IsValidImageFormat(string imageSource)
        {
            return Image.DetectFormat(imageSource) != null;
        }

        /// <summary>
        /// Returns true if the image blob is a supported image format
        /// </summary>
        /// <param name="imageBlob"></param>
        /// <returns></returns>
        public static bool IsValidImageFormat(byte[] imageBlob)
        {
            return Image.Identify(imageBlob) != null;
        }

        /// <summary>
        /// Returns a filter string for use with FileDialog filters
        /// </summary>
        /// <returns></returns>
        private static string GetSupportedFileDialogFilterString()
        {
            // Return the previous filter string if we've already retrieved it once before
            if (string.IsNullOrWhiteSpace(FileDialogSupportedImageFormats) == false)
            {
                return FileDialogSupportedImageFormats;
            }

            return string.Join("|", Configuration.Default.ImageFormats
                .Select(x => new
                {
                    Name = x.Name,
                    Extensions = string.Join(";", x.FileExtensions.Select(y => $"*.{y}"))
                })
                .Select(x => $"{x.Name} ({x.Extensions})|{x.Extensions}"));
        }

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
