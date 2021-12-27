using Nulah.PhantomIndex.Core;
using Nulah.PhantomIndex.Core.Controls;
using Nulah.PhantomIndex.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            this.DataContext = AppViewModel;
            /*
            var a = typeof(MainWindow).ResolvePageViewFromAssembly("Pages/Profiles/Index");

            if (a.PageView != null)
            {
                var b = NavigationContent.Navigate(a.PageViewParameters != null
                    ? Activator.CreateInstance(a.PageView, a.PageViewParameters)
                    : Activator.CreateInstance(a.PageView)
                );
            }
            */
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var s = (Button)sender;

        //    var a = typeof(MainWindow).ResolvePageViewFromAssembly(s.Tag as string);

        //    if (a.PageView != null)
        //    {
        //        var pageView = Activator.CreateInstance(a.PageView) as Page;

        //        //NavigationContent.Navigate(pageView, "asdf");

        //        /*
        //        NavigationContent.Navigate(a.PageViewParameters != null
        //            ? Activator.CreateInstance(a.PageView, a.PageViewParameters)
        //            : Activator.CreateInstance(a.PageView)
        //        );
        //        */
        //    }
        //}
    }
}
