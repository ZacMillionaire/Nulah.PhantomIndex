using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nulah.PhantomIndex.Core.Controls
{
    public class NulahIcon : UserControl
    {
        public FontIcon? Icon
        {
            get { return (FontIcon?)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(FontIcon?), typeof(NulahIcon), new PropertyMetadata(IconDependencyValueChanged));

        public event PropertyChangedEventHandler PropertyChanged;

        private static void IconDependencyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (NulahIcon)d;
            self.UpdateIcon((FontIcon)e.NewValue);
        }

        public string IconGlyph
        {
            get { return (string)GetValue(IconGlyphProperty); }
            // Readonly property for designer view
            private set { SetValue(IconGlyphProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconGlyph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconGlyphProperty =
            DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(NulahIcon), new PropertyMetadata(string.Empty));

        public void UpdateIcon(FontIcon icon)
        {
            IconGlyph = SegoeMDL2AssetsFontIcons.GetIcon(icon);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IconGlyph)));
        }

        static NulahIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NulahIcon), new FrameworkPropertyMetadata(typeof(NulahIcon)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
