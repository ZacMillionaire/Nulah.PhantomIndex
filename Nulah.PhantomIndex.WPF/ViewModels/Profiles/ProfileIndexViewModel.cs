using Nulah.PhantomIndex.Core.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

        private List<ProfileInfoShort> _profiles;

        public List<ProfileInfoShort> Profiles
        {
            get => _profiles;
            set => NotifyAndSetPropertyIfChanged(ref _profiles, value);
        }
    }
    public class ProfileInfoShort
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Pronouns { get; set; }
        public BitmapImage ProfileImage { get; set; }
        public string DisplayName1
        {
            get
            {
                return "asdf";
            }
        }
    }
}
