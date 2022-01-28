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
        public static readonly RoutedEvent NavigationItemClicked = EventManager.RegisterRoutedEvent(nameof(NavigationItemClicked), RoutingStrategy.Bubble, typeof(NulahNavigation), typeof(NavigationItem));

        // Why was whatever this was here?
        /*
        public static void AddNavigationItemClickedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie && uie != null)
            {
                uie.AddHandler(NavigationItemClicked, handler);
            }
        }

        public static void RemoveNavigationItemClickedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie && uie != null)
            {
                uie.RemoveHandler(NavigationItemClicked, handler);
            }
        }*/

        static NulahNavigation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NulahNavigation), new FrameworkPropertyMetadata(typeof(NulahNavigation)));
        }

        public NulahNavigation()
        {
            MenuItems = new();
            FooterMenuItems = new();
        }

        public List<NavigationLink> MenuItems
        {
            get { return (List<NavigationLink>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register(nameof(MenuItems), typeof(List<NavigationLink>), typeof(NulahNavigation), new PropertyMetadata(null));



        public List<NavigationLink> FooterMenuItems
        {
            get { return (List<NavigationLink>)GetValue(FooterMenuItemsProperty); }
            set { SetValue(FooterMenuItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FooterMenuItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterMenuItemsProperty =
            DependencyProperty.Register(nameof(FooterMenuItems), typeof(List<NavigationLink>), typeof(NulahNavigation), new PropertyMetadata(null));



        private Border _navigationFrame;

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("NavigationContent") is Border navigationFrame)
            {
                _navigationFrame = navigationFrame;
            }

            // Register the routed event for NavigationItems to the entire control
            AddHandler(NavigationItemClicked, new RoutedEventHandler(ChildNavigationClickEvent));

            base.OnApplyTemplate();
        }

        private void ChildNavigationClickEvent(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is NavigationItem source)
            {
                LoadPageFromNavigationItem(source.Tag as string);
            }
        }

        // future potential usercontrol caching
        //private Dictionary<string, UserControl> _navigationCache = new();

        private UserControl _currentUserControl = null;

        private void LoadPageFromNavigationItem(string navigationItemTag)
        {
            var pageViewFromAssembly = Assembly.GetEntryAssembly().ResolvePageViewFromAssembly(navigationItemTag);

            /*
            // future potential usercontrol caching
            if (_navigationCache.ContainsKey(navigationItemTag))
            {
                _navigationFrame.Child = _navigationCache[navigationItemTag];
                return;
            }
            */
            if (pageViewFromAssembly.PageView != null)
            {
                var pageView = PageViewResolver.GetActivatedPageViewByParameters<UserControl>(pageViewFromAssembly.PageView, pageViewFromAssembly.PageViewParameters);

                if (pageView != null)
                {
                    // Dispose the DataContext on a UserControl if it supports it
                    if (_currentUserControl != null && _currentUserControl.DataContext is IDisposable disposableDataContext)
                    {
                        disposableDataContext.Dispose();
                    }

                    // Ensure the page correctly inherits snapping and layout rounding
                    pageView.SnapsToDevicePixels = this.SnapsToDevicePixels;
                    pageView.UseLayoutRounding = this.UseLayoutRounding;

                    _currentUserControl = pageView;

                    _navigationFrame.Child = _currentUserControl;

                    // future potential usercontrol caching
                    //_navigationCache.Add(navigationItemTag, pageView);
                }
                else
                {
                    MessageBox.Show("Unable to find page view with parameter");
                }
            }
            else
            {
                MessageBox.Show("Unable to find page");
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        /// <summary>
        /// Sets the navigation content to the given location
        /// </summary>
        /// <param name="pageNavigationUri"></param>
        public void NavigateToPage(string pageNavigationUri)
        {
            LoadPageFromNavigationItem(pageNavigationUri);
        }
    }

    public class NavigationLink : UserControl
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

        public override void OnApplyTemplate()
        {
            var self = GetTemplateChild("NavigationContent") as NavigationLink;

            if (self != null)
            {
                self.MouseDown += Self_MouseDown;
            }

            base.OnApplyTemplate();
        }

        private void Self_MouseDown(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NulahNavigation.NavigationItemClicked, this));
        }
    }

    public class NavigationSeparator : NavigationLink
    {
        static NavigationSeparator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationSeparator),
                new FrameworkPropertyMetadata(typeof(NavigationSeparator)));
        }
    }

    public class NavigationItemCollapsable : NavigationLink, INotifyPropertyChanged
    {
        public List<NavigationLink> MenuItems
        {
            get { return (List<NavigationLink>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register(nameof(MenuItems), typeof(List<NavigationLink>), typeof(NavigationItemCollapsable), new PropertyMetadata(null));

        public bool Expanded
        {
            get { return (bool)GetValue(CollapsedProperty); }
            set { SetValue(CollapsedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Collapsed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CollapsedProperty =
            DependencyProperty.Register(nameof(Expanded), typeof(bool), typeof(NavigationItemCollapsable), new PropertyMetadata(CollapsedCallback));

        public event PropertyChangedEventHandler PropertyChanged;

        private static void CollapsedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = (NavigationItemCollapsable)sender;
            s.ChangeExpandCollapse();
        }

        public void ChangeExpandCollapse()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Expanded)));
        }

        public NavigationItemCollapsable()
        {
            MenuItems = new List<NavigationLink>();
        }

        static NavigationItemCollapsable()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationItemCollapsable),
                new FrameworkPropertyMetadata(typeof(NavigationItemCollapsable)));
        }

        public override void OnApplyTemplate()
        {
            var collapseHeader = GetTemplateChild("CollapseHeader") as UserControl;

            if (collapseHeader != null)
            {
                collapseHeader.MouseLeftButtonDown += CollapseHeader_MouseLeftButtonDown;
                //menuItems.SelectionChanged += SelectedItemChanged;
            }

            base.OnApplyTemplate();
        }

        private void CollapseHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Expanded = !Expanded;
            // Stop the event from bubbling
            e.Handled = true;
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
