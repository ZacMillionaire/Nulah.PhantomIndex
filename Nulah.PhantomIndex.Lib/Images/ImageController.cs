using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using Nulah.PhantomIndex.Lib.Images.Models;
using SQLite;

namespace Nulah.PhantomIndex.Lib.Images
{
    public class ImageController : DatabaseControllerBase
    {
        public readonly static string FileDialogSupportedImageFormats;

        internal string ImageTableName;
        internal string ImageResourceLinkTableName;

        static ImageController()
        {
            FileDialogSupportedImageFormats = GetSupportedFileDialogFilterString();
        }

        public ImageController(DatabaseManager databaseManager)
            : base(databaseManager)
        { }

        internal override void Init()
        {
            base.Init();

            // Create tables required for this controller
            Task.Run(() =>
            {
                DatabaseManager.Connection
                    !.CreateTableAsync<ImageResourceTable>()
                    .ConfigureAwait(false);
                DatabaseManager.Connection
                    !.CreateTableAsync<Image_ResourceTable>()
                    .ConfigureAwait(false);
            });

            ImageTableName = ((TableAttribute)typeof(ImageResourceTable).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(new TableAttribute("ImageResource"))).Name;
            ImageResourceLinkTableName = ((TableAttribute)typeof(Image_ResourceTable).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(new TableAttribute("Image_Resource"))).Name;
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
        /// Creates an <see cref="ImageResourceTable"/> entry for the given <paramref name="resourceId"/>.
        /// <para>
        /// A <paramref name="resourceId"/> is any entity that exists in another table.
        /// </para>
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="imageBlob"></param>
        /// <returns></returns>
        public async Task<ImageResourceTable> SaveImageForResource(Guid resourceId, byte[] imageBlob, string imageType)
        {
            if (imageBlob == null || imageBlob.Length == 0)
            {
                throw new ArgumentNullException($"{nameof(imageBlob)} cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(imageType) == true)
            {
                throw new ArgumentNullException($"{nameof(imageType)} cannot be null or empty");
            }

            var imageDetails = Image.DetectFormat(imageBlob);

            var newImage = new ImageResourceTable
            {
                Filesize = imageBlob.Length,
                ImageBlob = imageBlob,
                Mimetype = imageDetails.DefaultMimeType,
                Id = Guid.NewGuid(),
                Name = imageDetails.Name,
            };

            try
            {
                await DatabaseManager.Connection!.RunInTransactionAsync(async a =>
                    {
                        var imagesInserted = await DatabaseManager.Connection
                            !.InsertAsync(newImage)
                            .ConfigureAwait(false);

                        if (imagesInserted == 1)
                        {
                            var imageResourceLinked = await LinkImageToResource(newImage.Id, resourceId, imageType)
                                .ConfigureAwait(false);

                            if (imageResourceLinked == true)
                            {
                                a.Commit();
                            }
                        }
                    })
                    .ConfigureAwait(false);

                return newImage;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Connects a <see cref="ImageResourceTable"/> to any resouce by its <paramref name="resourceId"/>
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<bool> LinkImageToResource(Guid imageId, Guid resourceId, string resourceType)
        {
            var imageResourceLink = new Image_ResourceTable
            {
                ImageId = imageId,
                ResourceId = resourceId,
                Type = resourceType
            };

            var imagesInserted = await DatabaseManager.Connection
                !.InsertAsync(imageResourceLink)
                .ConfigureAwait(false);

            if (imagesInserted == 1)
            {
                return true;
            }

            // TODO: flesh this out to something more meaningful instead of a raw exception on failure
            throw new Exception($"Failed to create {nameof(Image_ResourceTable)}");
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
                using (var input = await Image.LoadAsync(imageSource).ConfigureAwait(false))
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
