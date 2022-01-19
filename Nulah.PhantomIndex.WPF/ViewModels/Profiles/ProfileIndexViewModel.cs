using Nulah.PhantomIndex.Core.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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

        private int _totalProfiles;
        public int TotalProfiles
        {
            get => _totalProfiles;
            set => NotifyAndSetPropertyIfChanged(ref _totalProfiles, value);
        }
    }

    public class ProfileInfoShort
    {
        public Guid Id { get; set; }
        public string ProfileName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Pronouns { get; set; } = string.Empty;
        public byte[]? ImageBlob { get; set; }
        public DateTime Created { get; set; }

        private Color? _profileColour;
        public Color? ProfileColour
        {
            get
            {
                if (_profileColour == null)
                {
                    var guidId = Id.ToByteArray();
                    _profileColour = Color.FromRgb(guidId[3], guidId[5], guidId[7]);
                }

                return _profileColour;
            }
        }

        private BitmapImage? _profileImage;

        public BitmapImage? ProfileImage
        {
            get
            {
                if (_profileImage == null && ImageBlob != null)
                {
                    _profileImage = Helpers.ImageByteArrayToBitmap(ImageBlob);
                }
                return _profileImage;
            }
        }

    }
}
