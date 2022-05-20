using Nulah.PhantomIndex.Core.ViewModels;
using Nulah.PhantomIndex.Plugin.RSSReader.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Plugin.RSSReader.ViewModels.Feeds
{
    internal class FeedListViewModel : ViewModelBase
    {
        private List<Feed> _feeds;

        public List<Feed> Feeds
        {
            get => _feeds;
            set => NotifyAndSetPropertyIfChanged(ref _feeds, value);
        }

#if DEBUG
        public FeedListViewModel()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                Feeds = new List<Feed>()
                {
                    new Feed()
                    {
                        Id = Guid.NewGuid(),
                        Category = "cat1",
                        Name = "RSS Feed 1",
                        Url = "http://localhost/nope.xml"
                    },
                    new Feed()
                    {
                        Id = Guid.NewGuid(),
                        Category = "cat1",
                        Name = "RSS Feed 2",
                        Url = "http://localhost/nope.xml"
                    }
                };
            }
        }
#endif
    }
}
