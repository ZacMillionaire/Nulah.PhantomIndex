using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private void DragDropControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1)
                {
                    FileSourceInput.Text = files[0];
                }
            }

            FileDropValid = null;
        }
    }
}
