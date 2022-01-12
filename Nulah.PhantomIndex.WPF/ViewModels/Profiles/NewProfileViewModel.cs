using Nulah.PhantomIndex.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Nulah.PhantomIndex.WPF.ViewModels.Profiles
{
    public class NewProfileViewModel : ViewModelBase
    {
        private string _profileName = string.Empty;
        private string _displayFirstName = string.Empty;
        private string? _displayLastName;
        private string? _fileName;
        private byte[]? _imageBlob;
        private string _pronouns = string.Empty;

        /// <summary>
        /// Required for a new profile
        /// </summary>
        [Required(ErrorMessage = "Profile name is required")]
        public string ProfileName
        {
            get => _profileName;
            set => NotifyAndSetPropertyIfChanged(ref _profileName, value);
        }

        [Required(ErrorMessage = "A name to display for first name is required")]
        public string DisplayFirstName
        {
            get => _displayFirstName;
            set => NotifyAndSetPropertyIfChanged(ref _displayFirstName, value);
        }

        public string? DisplayLastName
        {
            get => _displayLastName;
            set => NotifyAndSetPropertyIfChanged(ref _displayLastName, value);
        }

        public string? FileName
        {
            get => _fileName;
            set => NotifyAndSetPropertyIfChanged(ref _fileName, value);
        }

        public byte[]? ImageBlob
        {
            get => _imageBlob;
            set => NotifyAndSetPropertyIfChanged(ref _imageBlob, value);
        }

        private BitmapImage? _profileImage;
        public BitmapImage? ProfileImage
        {
            get => _profileImage;
            set => NotifyAndSetPropertyIfChanged(ref _profileImage, value);
        }

        [DefaultValue("They/Them")]
        public string Pronouns
        {
            get => _pronouns;
            set => NotifyAndSetPropertyIfChanged(ref _pronouns, value);
        }
    }
}
