using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nulah.PhantomIndex.Core.Controls
{
    public class NulahImage : UserControl
    {
        public BitmapImage Source
        {
            get { return (BitmapImage)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(BitmapImage), typeof(NulahImage), new PropertyMetadata(null));

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(double), typeof(NulahImage), new PropertyMetadata(SizePropertyCallback));

        private static void SizePropertyCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = (NulahImage)sender;

            s.ClipRadius = new CornerRadius(s.Size);
        }

        // Hide width and height - the Size property handles this
        public new int Width { get; }
        public new int Height { get; }

        public CornerRadius ClipRadius
        {
            get { return (CornerRadius)GetValue(ClipRadiusProperty); }
            private set { SetValue(ClipRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClipRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClipRadiusProperty =
            DependencyProperty.Register(nameof(ClipRadius), typeof(CornerRadius), typeof(NulahImage), new PropertyMetadata(null));

        static NulahImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NulahImage), new FrameworkPropertyMetadata(typeof(NulahImage)));
        }

        public override void OnApplyTemplate()
        {
            var gridClipMask = GetTemplateChild("ClipMask") as VisualBrush;

            if (gridClipMask != null)
            {
                gridClipMask.Visual = GetTemplateChild("VisualBorderClip") as Border;
            }

            base.OnApplyTemplate();
        }
    }
}
