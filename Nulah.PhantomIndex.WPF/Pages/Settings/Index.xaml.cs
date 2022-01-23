using Nulah.PhantomIndex.WPF.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nulah.PhantomIndex.WPF.Pages.Settings
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index : UserControl
    {
        private readonly SettingsViewModel _viewModel = new();

        public Index()
        {
            InitializeComponent();
            _viewModel.ApplicationDatabaseLocation = new FileInfo(App.Database.DatabaseLocation).DirectoryName;
            DataContext = _viewModel;
        }

        private void OpenExplorerButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", _viewModel.ApplicationDatabaseLocation);
        }
    }
}
