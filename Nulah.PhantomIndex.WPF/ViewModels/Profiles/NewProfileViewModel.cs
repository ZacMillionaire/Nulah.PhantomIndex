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
            get => _fileName;
            set => NotifyAndSetPropertyIfChanged(ref _fileName, value);
        }

        private string _profileName;

        public string ProfileName
        {
            get => _profileName;
            set => NotifyAndSetPropertyIfChanged(ref _profileName, value);
        }

        private byte[] _imageBlob;

        public byte[] ImageBlob
        {
            get => _imageBlob;
            set => NotifyAndSetPropertyIfChanged(ref _imageBlob, value);
        }


    }
}
