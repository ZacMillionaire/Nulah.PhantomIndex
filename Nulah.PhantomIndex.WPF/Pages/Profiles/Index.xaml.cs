using Nulah.PhantomIndex.WPF.ViewModels.Profiles;
using System;
using System.Collections.Generic;
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

namespace Nulah.PhantomIndex.WPF.Pages.Profiles
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        public ProfileIndexViewModel _viewModel = new();

        public Index()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        public Index(string viewParameters) : this()
        {
            _viewModel.PageName = viewParameters;
        }
    }
}
