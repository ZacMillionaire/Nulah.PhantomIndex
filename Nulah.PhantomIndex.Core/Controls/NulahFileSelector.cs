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

        public bool RaiseOnFileNameChange
        {
            get { return (bool)GetValue(RaiseOnFileNameChangeProperty); }
            set { SetValue(RaiseOnFileNameChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RaiseEventOnFileNameChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RaiseOnFileNameChangeProperty =
            DependencyProperty.Register(nameof(RaiseOnFileNameChange), typeof(bool), typeof(NulahFileSelector), new PropertyMetadata(false));

        private static void FileSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            if (e.NewValue is string newFileSource && string.IsNullOrWhiteSpace(newFileSource) == false)
            {
                ((NulahFileSelector)d).FileSourceChange(newFileSource);
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

        protected override void OnStyleChanged(Style oldStyle, Style newStyle)
        {
            base.OnStyleChanged(oldStyle, newStyle);
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
                // Set the flag to indicate the property change event came from code and not binding
                // to prevent FileSelected from being invoked twice - once here, then once as a result of FileSource being updated
                _fileLocationEventRaisedFromButton = true;

                FileSource = openFileDialog.FileName;
                FileSelected?.Invoke(this, FileSource);

                _fileLocationEventRaisedFromButton = false;
            }
        }

        /// <summary>
        /// Indicates if the file location changing was from button to prevent <see cref="FileSelected"/> from invoking twice.
        /// </summary>
        private bool _fileLocationEventRaisedFromButton = false;

        /// <summary>
        /// Raises the <see cref="FileSelected"/> event with the selected file location if <see cref="RaiseOnFileNameChange"/> is true
        /// </summary>
        /// <param name="newFileSource"></param>
        public void FileSourceChange(string newFileSource)
        {
            // Only invoke bound events if RaiseOnFileNameChange is true, and the property binding raising this event wasn't
            // a result of the file dialog button click event
            if (_fileLocationEventRaisedFromButton == false && RaiseOnFileNameChange == true)
            {
                FileSelected?.Invoke(this, FileSource);
            }
        }
    }
}
