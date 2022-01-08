using Microsoft.Win32;
using Nulah.PhantomIndex.Lib.Images;
using Nulah.PhantomIndex.WPF.ViewModels.Profiles;
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
        public NewProfileViewModel _viewModel = new();

        public NewProfile()
        {
            InitializeComponent();
            DataContext = _viewModel;
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
                if (files.Length == 1 && ImageController.IsValidImageFormat(files[0]) == true)
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
                    await ProcessProfileImage(files[0]);
                }
            }

            FileDropValid = null;
        }

        private async Task ProcessProfileImage(string imageSource)
        {
            if (string.IsNullOrWhiteSpace(imageSource) == true
                || ImageController.IsValidImageFormat(imageSource) == false)
            {
                return;
            }

            var resizedImageBlob = await ResizeImage(imageSource);

            _viewModel.ImageBlob = resizedImageBlob;
            _viewModel.FileName = imageSource;

            ImageDropCanvas.Source = ImageByteArrayToBitmap(_viewModel.ImageBlob);
        }

        private BitmapImage ImageByteArrayToBitmap(byte[] imageBlob)
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

        private async Task<byte[]> ResizeImage(string imageSource)
        {
            // Return a task here to ensure the UI is not blocked
            return await Task.Run(async () =>
            {
                var imageController = new ImageController();

                return await imageController.ResizeImageToWidth(imageSource, 300);
            });
        }

        private async void NulahFileSelector_FileSelected(object sender, string selectedFile)
        {
            await ProcessProfileImage(selectedFile);
        }

        private void CreateProfile_Click(object sender, RoutedEventArgs e)
        {
            var viewModelValid = _viewModel.Validate();
        }
    }


}
