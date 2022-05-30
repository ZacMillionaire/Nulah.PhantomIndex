using Nulah.PhantomIndex.Core;
using Nulah.PhantomIndex.Core.Controls;
using Nulah.PhantomIndex.Lib.Plugins;
using Nulah.PhantomIndex.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nulah.PhantomIndex.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NulahWindow
    {
        public AppViewModel AppViewModel = new();
        public static NulahNavigation Navigation;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = AppViewModel;

            Navigation = MainWindowNavigation;

            var plugins = App.PhantomIndexManager.GetPlugins(Navigation);

            foreach (NulahPlugin plugin in plugins)
            {
                plugin.OnPluginInitialise();

                Navigation.MenuItems.Add(BuildPluginNavigation(plugin));
            }
        }

        /// <summary>
        /// Builds navigation for a plugin for use with menus within the main application
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        private NavigationItemCollapsable BuildPluginNavigation(NulahPlugin plugin)
        {
            var pluginMenuGroup = new NavigationItemCollapsable(plugin.Name)
            {
                Icon = plugin.Icon
            };

            var pluginType = plugin.GetType();

            foreach (PluginMenuItem pluginItem in plugin.Pages)
            {
                if (pluginItem is PluginMenuCategory subPage && subPage.Pages != null && subPage.Pages.Count != 0)
                {
                    var pluginSubMenuItem = new NavigationItemCollapsable(subPage.DisplayName)
                    {
                        Icon = subPage.Icon
                    };
                    BuildNavigationFromPlugin(pluginSubMenuItem, subPage.Pages, pluginType);
                    pluginMenuGroup.MenuItems.Add(pluginSubMenuItem);
                }
                else
                {
                    var navigationItem = new NavigationItem(pluginItem.DisplayName, pluginItem.PageLocation)
                    {
                        Icon = pluginItem.Icon,
                        NavigationSourceType = pluginType
                    };
                    pluginMenuGroup.MenuItems.Add(navigationItem);
                }
            }

            return pluginMenuGroup;
        }

        /// <summary>
        /// Builds any sub navigation items for a plugin.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="pluginNavigationItems"></param>
        /// <param name="pluginType"></param>
        private void BuildNavigationFromPlugin(NavigationItemCollapsable parent, List<PluginMenuItem> pluginNavigationItems, Type pluginType)
        {
            foreach (PluginMenuItem pluginItem in pluginNavigationItems)
            {
                if (pluginItem is PluginMenuCategory subPage && subPage.Pages != null && subPage.Pages.Count != 0)
                {
                    var pluginSubMenuItem = new NavigationItemCollapsable(subPage.DisplayName)
                    {
                        Icon = subPage.Icon
                    };
                    BuildNavigationFromPlugin(pluginSubMenuItem, subPage.Pages, pluginType);
                    parent.MenuItems.Add(pluginSubMenuItem);
                }
                else
                {
                    var navigationItem = new NavigationItem(pluginItem.DisplayName, pluginItem.PageLocation)
                    {
                        Icon = pluginItem.Icon,
                        NavigationSourceType = pluginType
                    };
                    parent.MenuItems.Add(navigationItem);
                }
            }
        }

        private void SettingsTitleButton_MouseDown(object sender, RoutedEventArgs e)
        {
            Navigation.NavigateToPage("Pages/Settings/Index");
        }
    }
}
