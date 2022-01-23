using Nulah.PhantomIndex.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.WPF.ViewModels.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _applicationDatabaseLocation;
        public string ApplicationDatabaseLocation
        {
            get => _applicationDatabaseLocation;
            set => NotifyAndSetPropertyIfChanged(ref _applicationDatabaseLocation, value);
        }
    }
}
