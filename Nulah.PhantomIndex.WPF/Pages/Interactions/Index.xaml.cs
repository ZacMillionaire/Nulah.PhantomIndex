using Nulah.PhantomIndex.WPF.ViewModels.Interactions;
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

namespace Nulah.PhantomIndex.WPF.Pages.Interactions
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index : UserControl
    {
        private readonly InteractionIndexViewModel _viewModel = new();

        public Index()
        {
            InitializeComponent();
            DataContext = _viewModel;

            Task.Run(async () =>
            {
                _viewModel.EventTypes = await App.Database.Events.GetEventTypes();
            });



        }

        private void InteractionGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            InteractionGrid.CancelEdit();
        }

        private void InteractionGrid_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
        {
            if(sender is DataGrid dg)
            {
                dg.BeginEdit();
            }
        }
    }
}
