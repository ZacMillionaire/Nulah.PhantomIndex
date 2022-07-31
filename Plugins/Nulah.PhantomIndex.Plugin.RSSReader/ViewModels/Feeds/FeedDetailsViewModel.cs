using Nulah.PhantomIndex.Core.ViewModels;
using Nulah.PhantomIndex.Plugin.RSSReader.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Plugin.RSSReader.ViewModels.Feeds
{
    internal class FeedDetailsViewModel : ViewModelBase
    {

        private Guid _id;

        public Guid Id
        {
            get => _id;
            set => NotifyAndSetPropertyIfChanged(ref _id, value);
        }

        private RSSFeed _feed;

        public RSSFeed Feed
        {
            get => _feed;
            set => NotifyAndSetPropertyIfChanged(ref _feed, value);
        }

#if DEBUG
        public FeedDetailsViewModel()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                Feed = new RSSFeed
                {
                    Description = "RSS DESCRIPTION",
                    LastUpdate = DateTime.Now,
                    Title = "RSS TITLE",
                    Items = new List<FeedItem>
                    {
                        new()
                        {
                            Title = "FEED ITEM TITLE",
                            Categories = new(){"CATEGORY 1","CATEGORY 2"},
                            Id = "http://localhost/feed-item-1",
                            Published = DateTime.Now,
                            Updated = DateTime.Now.AddDays(1),
                            Summary = "FEED ITEM SUMMARY SHORT"
                        },
                        new()
                        {
                            Title = "FEED ITEM 2 TITLE",
                            Categories = new(){"CATEGORY 1","CATEGORY 2", "CATEGORY 3"},
                            Id = "http://localhost/feed-item-2",
                            Published = DateTime.Now,
                            Updated = DateTime.Now.AddDays(1),
                            Summary = "FEED ITEM SUMMARY LONG FORM WITH A LOT OF TEXT BECAUSE HOLY SHIT THEY PUT THE ENTIRE ARTICLE IN HERE FOR SOME REASON"
                        }
                    }
                };

                Feed.Categories.AddRange(new List<string>() { "CATEGORY 1", "CATEGORY 2", "CATEGORY 3" });
            }
        }
#endif
    }
}
