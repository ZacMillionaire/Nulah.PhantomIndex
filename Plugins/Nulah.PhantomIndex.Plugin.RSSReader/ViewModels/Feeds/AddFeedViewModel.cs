using Nulah.PhantomIndex.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Plugin.RSSReader.ViewModels.Feeds
{
    internal class AddFeedViewModel : ViewModelBase
    {
        private string _name;
        private string _url;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Display name is required")]
        public string Name
        {
            get => _name;
            set => NotifyAndSetPropertyIfChanged(ref _name, value);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Feed URL is required")]
        public string Url
        {
            get => _url;
            set => NotifyAndSetPropertyIfChanged(ref _url, value);
        }

        public Guid Id { get; set; }
    }
}
