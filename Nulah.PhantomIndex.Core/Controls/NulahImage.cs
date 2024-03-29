﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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


        public Color BackgroundColour
        {
            get { return (Color)GetValue(BackgroundColourProperty); }
            set
            {
                SetValue(BackgroundColourProperty, value);
                if (value != default(Color))
                {
                    SetGradientBackground();
                }
            }
        }

        // Using a DependencyProperty as the backing store for StartColour.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundColourProperty =
            DependencyProperty.Register(nameof(BackgroundColour), typeof(Color), typeof(NulahImage), new PropertyMetadata(default(Color), BackgroundColourChanged));

        private static void BackgroundColourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((Color)e.NewValue != default(Color))
            {
                ((NulahImage)d).SetGradientBackground();
            }
        }

        public Color BorderColour
        {
            get { return (Color)GetValue(BorderColourProperty); }
            set { SetValue(BorderColourProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderColour.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderColourProperty =
            DependencyProperty.Register(nameof(BorderColour), typeof(Color), typeof(NulahImage), new PropertyMetadata(default(Color), BorderColourChanged));

        private static void BorderColourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((Color)e.NewValue != default(Color))
            {
                ((NulahImage)d).SetBorderBrush();
            }
        }

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

            //SetGradientBackground();
            //SetBorderBrush();

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Sets the colours for the gradient background given <see cref="BackgroundColour"/> and <see cref="GradientDark"/>.
        /// <para>
        /// This is done as a <see cref="LinearGradientBrush"/> cannot be easily bound to template bindings so it is created in code
        /// </para>
        /// </summary>
        private void SetGradientBackground()
        {
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1)
            };
            gradientBrush.GradientStops.Add(new GradientStop { Color = ControlHelpers.ChangeColorBrightness(BackgroundColour, 0.25f) });
            gradientBrush.GradientStops.Add(new GradientStop { Color = BackgroundColour, Offset = 0.5 });
            gradientBrush.GradientStops.Add(new GradientStop { Color = ControlHelpers.ChangeColorBrightness(BackgroundColour, -0.25f), Offset = 1 });

            gradientBrush.Freeze();

            Background = gradientBrush;
        }

        /// <summary>
        /// Sets the colour of the border brush based on <see cref="BorderColour"/>
        /// </summary>
        private void SetBorderBrush()
        {
            // We do this here as template binding a colour to a BorderBrush will fail to convert if the colour is not passed in as a colour string,
            // or a SolidColorBrush resource. A custom converter could be used but that is open to issues getting bindings correct in a ControlTemplate
            // and this essentially does the same thing
            BorderBrush = new SolidColorBrush(BorderColour);
        }
    }
}
