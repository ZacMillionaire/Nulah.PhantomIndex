using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Nulah.PhantomIndex.Core.Controls
{
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



        public double LabelSize
        {
            get { return (double)GetValue(LabelSizeProperty); }
            set { SetValue(LabelSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelSizeProperty =
            DependencyProperty.Register("LabelSize", typeof(double), typeof(NulahInput), new PropertyMetadata(null));


        public double InputSize
        {
            get { return (int)GetValue(InputSizeProperty); }
            set { SetValue(InputSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputSizeProperty =
            DependencyProperty.Register("InputSize", typeof(double), typeof(NulahInput), new PropertyMetadata(null));


        /// <summary>
        /// Prevent FontSize from being used
        /// </summary>
        public new int FontSize { get; private set; }


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
