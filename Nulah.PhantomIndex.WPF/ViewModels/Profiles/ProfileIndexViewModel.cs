using Nulah.PhantomIndex.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.WPF.ViewModels.Profiles
{
    public class ProfileIndexViewModel : ViewModelBase
    {
        private string _pageName = "Default";

        public string PageName
        {
            get { return _pageName; }
            set { NotifyAndSetPropertyIfChanged(ref _pageName, value); }
        }
    }
}
