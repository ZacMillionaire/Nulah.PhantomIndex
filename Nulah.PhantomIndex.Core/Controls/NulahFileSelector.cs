using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Nulah.PhantomIndex.Core.Controls
{
    public class NulahFileSelector : UserControl
    {
        public string FileSource
        {
            get { return (string)GetValue(FileSourceProperty); }
            set { SetValue(FileSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UploadedImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileSourceProperty =
            DependencyProperty.Register(nameof(FileSource), typeof(string), typeof(NulahFileSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FileSourcePropertyChanged)
            {
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

        private static void FileSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            if (e.NewValue is string newFileSource && string.IsNullOrWhiteSpace(newFileSource) == false)
            {
                ((NulahFileSelector)d).RaiseFileSourceChange(newFileSource);
            }
        }

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Filter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(nameof(Filter), typeof(string), typeof(NulahFileSelector), new PropertyMetadata("All Files (*.*)|*.*"));


        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register(nameof(ButtonContent), typeof(string), typeof(NulahFileSelector), new PropertyMetadata("Choose"));


        public event EventHandler<string> FileSelected;

        static NulahFileSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NulahFileSelector), new FrameworkPropertyMetadata(typeof(NulahFileSelector)));
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("FileDialogButton") is Button fileDialogButton && fileDialogButton != null)
            {
                fileDialogButton.Click += FileDialogButton_Click;
            }
            base.OnApplyTemplate();
        }

        private void FileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = Filter;

            if (openFileDialog.ShowDialog() == true)
            {
                RaiseFileSourceChange(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Updates the value of <see cref="FileSource"/> updating any bindings to it, and raises the <see cref="FileSelected"/> event with the selected file location
        /// </summary>
        /// <param name="newFileSource"></param>
        public void RaiseFileSourceChange(string newFileSource)
        {
            FileSource = newFileSource;
            FileSelected?.Invoke(this, FileSource);
        }
    }
}
