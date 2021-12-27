using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nulah.PhantomIndex.Core.Controls
{
    public class NulahNavigation : UserControl
    {
        static NulahNavigation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NulahNavigation),
                new FrameworkPropertyMetadata(typeof(NulahNavigation)));
        }

        public NulahNavigation()
        {
            MenuItems = new();
        }

        public List<NavigationLink> MenuItems
        {
            get { return (List<NavigationLink>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register("MenuItems", typeof(List<NavigationLink>), typeof(NulahNavigation), new PropertyMetadata(null));

        public event EventHandler ItemSelected;
        private Frame _navigationFrame;
        private Delegate routedItemEvent;

        public override void OnApplyTemplate()
        {
            var menuItems = GetTemplateChild("MenuItems") as ItemsControl;

            if (menuItems != null)
            {
                //menuItems.SelectionChanged += SelectedItemChanged;
            }

            var navigationFrame = GetTemplateChild("NavigationContent") as Frame;
            if (navigationFrame != null)
            {
                _navigationFrame = navigationFrame;
            }

            base.OnApplyTemplate();
        }

        private void SelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((ListView)sender).SelectedItem;
            var a = Assembly.GetEntryAssembly().ResolvePageViewFromAssembly(((UserControl)selectedItem).Tag as string);

            if (a.PageView != null)
            {
                var pageView = PageViewResolver.GetActivatedPageViewByParameters(a.PageView, a.PageViewParameters);

                if (pageView != null)
                {
                    _navigationFrame.Navigate(pageView, "asdf");
                }
                else
                {
                    MessageBox.Show("Unable to find page view with parameter");
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
    }

    public abstract class NavigationLink : UserControl
    {
        public FontIcon? Icon
        {
            get { return (FontIcon?)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(FontIcon?), typeof(NavigationLink), new PropertyMetadata(null));

        static NavigationLink()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationLink),
                new FrameworkPropertyMetadata(typeof(NavigationLink)));
        }
    }

    public class NavigationHeader : NavigationLink
    {
        static NavigationHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationHeader), new FrameworkPropertyMetadata(typeof(NavigationHeader)));
        }
    }

    public class NavigationItem : NavigationLink
    {
        static NavigationItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationItem),
                new FrameworkPropertyMetadata(typeof(NavigationItem)));
        }
    }

    public class NavigationItemCollapsable : NavigationLink, INotifyPropertyChanged
    {
        public List<NavigationItem> MenuItems
        {
            get { return (List<NavigationItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register(nameof(MenuItems), typeof(List<NavigationItem>), typeof(NavigationItemCollapsable), new PropertyMetadata(null));

        public bool Collapsed
        {
            get { return (bool)GetValue(CollapsedProperty); }
            set { SetValue(CollapsedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Collapsed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CollapsedProperty =
            DependencyProperty.Register(nameof(Collapsed), typeof(bool), typeof(NavigationItemCollapsable), new PropertyMetadata(CollapsedCallback));

        public event PropertyChangedEventHandler PropertyChanged;

        private static void CollapsedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = (NavigationItemCollapsable)sender;
            s.ChangeExpandCollapse((bool)e.NewValue);

        }

        public void ChangeExpandCollapse(bool expandCollapseState)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Collapsed)));
            _menuItems.Visibility = expandCollapseState ? Visibility.Visible : Visibility.Collapsed;
        }

        public NavigationItemCollapsable()
        {
            MenuItems = new List<NavigationItem>();
        }

        static NavigationItemCollapsable()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationItemCollapsable),
                new FrameworkPropertyMetadata(typeof(NavigationItemCollapsable)));
        }

        private ListView _menuItems;

        public override void OnApplyTemplate()
        {
            var collapseHeader = GetTemplateChild("CollapseHeader") as UserControl;

            if (collapseHeader != null)
            {
                collapseHeader.MouseLeftButtonDown += CollapseHeader_MouseLeftButtonDown;
                //menuItems.SelectionChanged += SelectedItemChanged;
            }

            _menuItems = GetTemplateChild("NestedItems") as ListView;

            base.OnApplyTemplate();
        }

        private void CollapseHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Collapsed = !Collapsed;
        }
    }

    public class NavigationIcon : UserControl, INotifyPropertyChanged
    {
        public FontIcon? Icon
        {
            get { return (FontIcon?)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(FontIcon?), typeof(NavigationIcon), new PropertyMetadata(IconDependencyValueChanged));

        public event PropertyChangedEventHandler PropertyChanged;

        private static void IconDependencyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (NavigationIcon)d;
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
            DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(NavigationIcon), new PropertyMetadata(string.Empty));

        public void UpdateIcon(FontIcon icon)
        {
            IconGlyph = SegoeMDL2AssetsFontIcons.GetIcon(icon);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IconGlyph)));
        }

        // Hide the base Content property to make it read only - this controls content is set via IconGlyph 
        public new object Content { get; private set; }


        public NavigationIcon()
        {
        }

        static NavigationIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationIcon),
                new FrameworkPropertyMetadata(typeof(NavigationIcon)));
        }
    }

    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
        }
    }
}
