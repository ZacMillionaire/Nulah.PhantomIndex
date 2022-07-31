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

        public MainWindow()
        {
            InitializeComponent();

            DataContext = AppViewModel;

            var plugins = App.PhantomIndexManager.GetPlugins();

            foreach (NulahPlugin plugin in plugins)
            {
                var navigationItem = new NavigationItem(plugin.Name, plugin.EntryPage)
                {
                    Icon = plugin.Icon,
                    NavigationSourceType = plugin.GetType()
                };

                MainWindowNavigation.MenuItems.Add(navigationItem);

                MenuBar.Items.Add(BuildPluginMenu(plugin));
            }
        }

        private MenuItem BuildPluginMenu(NulahPlugin plugin)
        {
            var baseMenuItem = new MenuItem()
            {
                Header = plugin.Name
            };

            // TODO: menu commands won't update the main navigation frame to the plugin, maybe look at wrapping the command with another so it does?
            // or maybe just relegate the menu to the plugins themselves and not have navigation
            // or maybe pass in the main navigation component as well as a command parameter for a plugincommand?
            foreach (PluginMenuItem pluginItem in plugin.Pages)
            {
                // TODO: handle submenus
                if (pluginItem is PluginMenuCategory subPage && subPage.Pages != null && subPage.Pages.Count != 0)
                {
                    //var pluginSubMenuItem = new MenuItem()
                    //{
                    //    Icon = subPage.Icon,
                    //    Header = subPage.DisplayName,
                    //    Tag = subPage.
                    //};
                    //BuildNavigationFromPlugin(pluginSubMenuItem, subPage.Pages, pluginType);
                    //pluginMenuGroup.MenuItems.Add(pluginSubMenuItem);
                }
                else
                {
                    var navigationItem = new MenuItem()
                    {
                        Icon = pluginItem.Icon,
                        Header = pluginItem.DisplayName,
                        Tag = pluginItem.PageLocation,
                        Command = pluginItem.Command,
                        CommandParameter = pluginItem.PageLocation
                    };

                    baseMenuItem.Items.Add(navigationItem);
                }
            }

            return baseMenuItem;
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
            MainWindowNavigation.NavigateToPage("Pages/Settings/Index");
        }
    }
}
