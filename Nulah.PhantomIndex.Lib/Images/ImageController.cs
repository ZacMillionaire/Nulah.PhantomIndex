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
        private readonly PhantomIndexDatabase _database;

        static ImageController()
        {
            FileDialogSupportedImageFormats = GetSupportedFileDialogFilterString();
        }

        public ImageController(PhantomIndexDatabase phantomIndexDatabase)
        {
            _database = phantomIndexDatabase;
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
        /// Determines the new size of an image to fit the target width.
        /// <para>
        /// If the <paramref name="originalWidth"/> is larger than <paramref name="targetWidth"/>, <paramref name="originalHeight"/> will be scaled to the required ratio.
        /// </para>
        /// <para>
        /// Images that are already smaller than the <paramref name="targetWidth"/> will be unchanged
        /// </para>
        /// </summary>
        /// <param name="targetWidth"></param>
        /// <param name="originalWidth"></param>
        /// <param name="originalHeight"></param>
        /// <returns></returns>
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
