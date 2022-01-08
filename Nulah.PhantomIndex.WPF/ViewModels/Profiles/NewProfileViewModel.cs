using Nulah.PhantomIndex.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.WPF.ViewModels.Profiles
{
    public class NewProfileViewModel : ViewModelBase
    {

        private string _profileName;

        /// <summary>
        /// Required for a new profile
        /// </summary>
        [Required(ErrorMessage = "Profile name is required")]
        public string ProfileName
        {
            get => _profileName;
            set => NotifyAndSetPropertyIfChanged(ref _profileName, value);
        }

        private string _displayFirstName;

        [Required(ErrorMessage = "A name to display for first name is required")]
        public string DisplayFirstName
        {
            get => _displayFirstName;
            set => NotifyAndSetPropertyIfChanged(ref _displayFirstName, value);
        }

        private string _displayLastName;

        public string DisplayLastName
        {
            get => _displayLastName;
            set => NotifyAndSetPropertyIfChanged(ref _displayLastName, value);
        }

        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set => NotifyAndSetPropertyIfChanged(ref _fileName, value);
        }

        private byte[] _imageBlob;

        public byte[] ImageBlob
        {
            get => _imageBlob;
            set => NotifyAndSetPropertyIfChanged(ref _imageBlob, value);
        }
    }
}
