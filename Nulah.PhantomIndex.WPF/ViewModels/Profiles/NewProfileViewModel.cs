using Nulah.PhantomIndex.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.WPF.ViewModels.Profiles
{
    public class NewProfileViewModel : ViewModelBase
    {
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { NotifyAndSetPropertyIfChanged(ref _fileName, value); }
        }
    }
}
