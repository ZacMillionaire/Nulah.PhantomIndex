using Nulah.PhantomIndex.Lib.Images;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nulah.PhantomIndex.WPF.Pages.Profiles
{
    /// <summary>
    /// Interaction logic for NewProfile.xaml
    /// </summary>
    public partial class NewProfile : Page, INotifyPropertyChanged
    {
        public NewProfile()
        {
            InitializeComponent();
        }



        public bool? FileDropValid
        {
            get { return (bool?)GetValue(FileDropValidProperty); }
            set { SetValue(FileDropValidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileDropValid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileDropValidProperty =
            DependencyProperty.Register(nameof(FileDropValid), typeof(bool?), typeof(NewProfile), new PropertyMetadata(null));

        public event PropertyChangedEventHandler? PropertyChanged;

        private void DragDropControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1)
                {
                    e.Effects = DragDropEffects.Copy;
                    FileDropValid = true;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                    FileDropValid = false;
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Required to display the correct cursor when draging from another window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragDropControl_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1)
                {
                    e.Effects = DragDropEffects.Copy;
                    FileDropValid = true;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                    FileDropValid = false;
                }

                e.Handled = true;
            }
        }

        private void DragDropControl_DragLeave(object sender, DragEventArgs e)
        {
            FileDropValid = null;
        }

        private async void DragDropControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1)
                {
                    ImageDropCanvas.Source = await ImageToBitmap(files[0]);
                }
            }

            FileDropValid = null;
        }

        private async Task<BitmapImage> ImageToBitmap(string imageSource)
        {
            // Return a task here to ensure the UI is not blocked
            return await Task.Run(async () =>
            {
                var imageController = new ImageController();

                var resizedImageData = await imageController.ResizeImageToWidth(imageSource, 300);
                var image = new BitmapImage();

                using (var mem = new MemoryStream(resizedImageData))
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
            });
        }
    }
}
