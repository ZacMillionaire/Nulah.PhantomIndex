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
                var pluginMenuSection = new NavigationItemCollapsable(plugin.Name);

                foreach (KeyValuePair<string, (FontIcon?, string)> navigationItem in plugin.Pages)
                {
                    if (navigationItem.Value.Item1.HasValue)
                    {
                        pluginMenuSection.MenuItems.Add(new NavigationItem(navigationItem.Value.Item2, navigationItem.Key, navigationItem.Value.Item1.Value)
                        {
                            NavigationSourceType = plugin.GetType()
                        });
                    }
                    else
                    {
                        pluginMenuSection.MenuItems.Add(new NavigationItem(navigationItem.Value.Item2, navigationItem.Key)
                        {
                            NavigationSourceType = plugin.GetType()
                        });
                    }
                }


                Navigation.MenuItems.Add(pluginMenuSection);
            }
        }
    }
}
