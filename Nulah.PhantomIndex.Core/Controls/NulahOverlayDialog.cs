using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nulah.PhantomIndex.Core.Controls
{
    public class NulahOverlayDialog : UserControl
    {


        public bool ClosesOnBackgroundClick
        {
            get { return (bool)GetValue(ClosesOnBackgroundClickProperty); }
            set { SetValue(ClosesOnBackgroundClickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClosesOnBackgroundClick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClosesOnBackgroundClickProperty =
            DependencyProperty.Register(nameof(ClosesOnBackgroundClick), typeof(bool), typeof(NulahOverlayDialog), new PropertyMetadata(false));



        public NulahOverlayDialog()
            : base()
        {
        }

        static NulahOverlayDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NulahOverlayDialog), new FrameworkPropertyMetadata(typeof(NulahOverlayDialog)));
        }

        private DialogPanel _backingDialogPanel;

        public override void OnApplyTemplate()
        {
            _backingDialogPanel = GetTemplateChild("DialogBackground") as DialogPanel;
            if (_backingDialogPanel != null)
            {
                _backingDialogPanel.MouseDown += DialogPanel_MouseDown;
            }

            base.OnApplyTemplate();
        }

        private void DialogPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource == _backingDialogPanel)
            {
                if (ClosesOnBackgroundClick == false)
                {
                    return;
                }
                Visibility = Visibility.Collapsed;
            }
        }
    }

    public class DialogPanel : Panel
    {
        public DialogPanel()
            : base() { }

        private void DialogPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource == this)
            {
                Visibility = Visibility.Collapsed;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size panelDesiredSize = new Size();

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
                panelDesiredSize = child.DesiredSize;
            }

            return panelDesiredSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {

            foreach (UIElement child in InternalChildren)
            {
                child.Arrange(new Rect(new Point(0, (finalSize.Height / 2) - child.DesiredSize.Height / 2), new Size(finalSize.Width, child.DesiredSize.Height)));
            }
            return finalSize;
        }
    }
}
