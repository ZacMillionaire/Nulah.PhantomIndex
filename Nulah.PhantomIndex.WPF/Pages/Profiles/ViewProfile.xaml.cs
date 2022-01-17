using Nulah.PhantomIndex.Core;
using Nulah.PhantomIndex.Lib.Profiles.Models;
using Nulah.PhantomIndex.WPF.ViewModels.Profiles;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ViewProfile.xaml
    /// </summary>
    public partial class ViewProfile : UserControl
    {
        private ViewProfileViewModel _viewModel = new();

        internal ViewProfile()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        public ViewProfile(string profileId) : this()
        {
            if (Guid.TryParse(profileId, out Guid validProfileId) == false)
            {
                throw new Exception("Invalid profile Id format");
            }

            Task.Run(async () => await LoadProfileById(validProfileId));
        }

        private async Task LoadProfileById(Guid profileId)
        {
            await Task.Run(async () =>
            {
                var profile = await App.Database.Profiles.GetProfile(profileId);
                if (profile != null)
                {
                    _viewModel.Id = profile.Id;
                    _viewModel.DisplayFirstName = profile.DisplayFirstName;
                    _viewModel.DisplayLastName = profile.DisplayLastName;
                    _viewModel.Name = profile.Name;
                    _viewModel.Pronouns = profile.Pronouns;
                    _viewModel.ImageBlob = profile.ImageBlob;
                }
            });
        }
    }


    public class ColourEnhancerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color && color != default(Color))
            {
                if (parameter is string brightnessString && float.TryParse(brightnessString, out float brightness))
                {
                    return ControlHelpers.ChangeColorBrightness(color, brightness);
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
