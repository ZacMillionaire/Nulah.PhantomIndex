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
                    _viewModel.FileName = files[0];
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

    public class NulahInput : UserControl
    {
        /// <summary>
        /// Label
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(NulahInput), new PropertyMetadata(null));

        /// <summary>
        /// Optional text to describe the input.
        /// </summary>
        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HintForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register(nameof(Hint), typeof(string), typeof(NulahInput), new PropertyMetadata(null));

        /// <summary>
        /// Foreground colour for input hint
        /// </summary>
        public Brush HintForeground
        {
            get { return (Brush)GetValue(HintForegroundProperty); }
            set { SetValue(HintForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HintForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintForegroundProperty =
            DependencyProperty.Register(nameof(HintForeground), typeof(Brush), typeof(NulahInput), new PropertyMetadata(null));


        /// <summary>
        /// Binding value for input value
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(NulahInput),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                {
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        /// <summary>
        /// If true, the binding source will be updated on any change to InputBox, otherwise change will occur on InputBox LostFocus
        /// </summary>
        public bool UpdateValueOnKeyUp
        {
            get { return (bool)GetValue(UpdateValueOnKeyUpProperty); }
            set { SetValue(UpdateValueOnKeyUpProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateOnKeyUp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateValueOnKeyUpProperty =
            DependencyProperty.Register(nameof(UpdateValueOnKeyUp), typeof(bool), typeof(NulahInput), new PropertyMetadata(false, UpdateValueOnKeyUpPropertyChanged));

        private static void UpdateValueOnKeyUpPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((NulahInput)d).SetInputKeyUpHandler((bool)e.NewValue);
            }
        }

        static NulahInput()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NulahInput), new FrameworkPropertyMetadata(typeof(NulahInput)));
        }

        /// <summary>
        /// Used for when <see cref="UpdateValueOnKeyUp"/> is true to raise binding updates on keydown
        /// </summary>
        private BindingExpression _textboxBindingExpression;

        public override void OnApplyTemplate()
        {
            _textboxBindingExpression = BindingOperations.GetBindingExpression(GetTemplateChild("InputBox"), TextBox.TextProperty);

            SetInputKeyUpHandler(UpdateValueOnKeyUp);
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Adds/Removes <see cref="Input_KeyUp(object, KeyEventArgs)"/> from the InputBox's <see cref="UIElement.KeyUp"/> event
        /// </summary>
        /// <param name="addHandler"></param>
        private void SetInputKeyUpHandler(bool addHandler)
        {
            if (GetTemplateChild("InputBox") is TextBox input && input != null)
            {
                if (addHandler == true)
                {
                    input.KeyUp += Input_KeyUp;
                }
                else
                {
                    input.KeyUp -= Input_KeyUp;
                }
            }
        }

        private void Input_KeyUp(object sender, KeyEventArgs e)
        {
            _textboxBindingExpression?.UpdateSource();
        }
    }
}
