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
        public static Core.Controls.NulahNavigation Navigation;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = AppViewModel;

            Navigation = MainWindowNavigation;

            var plugins = App.PhantomIndexManager.GetPlugins();

            foreach (IPlugin plugin in plugins)
            {
                var pluginMenuSection = new NavigationItemCollapsable(plugin.Name)
                {
                    Icon = plugin.Icon
                };

                BuildNavigationFromPlugin(pluginMenuSection, plugin.Pages, plugin.GetType());

                Navigation.MenuItems.Add(pluginMenuSection);
            }
        }

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
    }
}
