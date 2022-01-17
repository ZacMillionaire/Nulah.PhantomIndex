using Nulah.PhantomIndex.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nulah.PhantomIndex.WPF.ViewModels.Profiles
{
    public class ViewProfileViewModel : ViewModelBase
    {
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set
            {
                NotifyAndSetPropertyIfChanged(ref _id, value);

                // Set the profile colour
                var guidId = _id.ToByteArray();
                ProfileColour = Color.FromRgb(guidId[3], guidId[5], guidId[7]);
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => NotifyAndSetPropertyIfChanged(ref _name, value);
        }

        private string _displayFirstName = string.Empty;
        public string DisplayFirstName
        {
            get => _displayFirstName;
            set => NotifyAndSetPropertyIfChanged(ref _displayFirstName, value);
        }

        private string? _displayLastName;
        public string? DisplayLastName
        {
            get => _displayLastName;
            set => NotifyAndSetPropertyIfChanged(ref _displayLastName, value);
        }

        private string _pronouns = string.Empty;
        public string Pronouns
        {
            get => _pronouns;
            set => NotifyAndSetPropertyIfChanged(ref _pronouns, value);
        }

        private byte[]? _imageBlob;
        public byte[]? ImageBlob
        {
            get => _imageBlob;
            set => NotifyAndSetPropertyIfChanged(ref _imageBlob, value);
        }

        private Color _profileColour;
        public Color ProfileColour
        {
            get => _profileColour;
            set => NotifyAndSetPropertyIfChanged(ref _profileColour, value);
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
