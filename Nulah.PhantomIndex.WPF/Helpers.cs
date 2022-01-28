using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Nulah.PhantomIndex.WPF
{
    internal static class Helpers
    {
        /// <summary>
        /// Takes a byte[] of an image and returns a BitmapImage
        /// </summary>
        /// <param name="imageBlob"></param>
        /// <returns></returns>
        public static BitmapImage ImageByteArrayToBitmap(byte[] imageBlob)
        {
            var image = new BitmapImage();

            using (var mem = new MemoryStream(imageBlob))
            {
                mem.Position = 0;
                image.BeginInit();

                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = mem;

                image.EndInit();
            }

            image.Freeze();
            return image;
        }
    }
}
