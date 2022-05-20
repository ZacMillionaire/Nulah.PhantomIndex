using Nulah.PhantomIndex.Plugin.RSSReader.Data.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Nulah.PhantomIndex.Plugin.RSSReader.Data
{
    internal class RSSFeeder
    {
        internal readonly SQLiteAsyncConnection Connection;

        public RSSFeeder(string dbLocation)
        {
            Connection = new SQLiteAsyncConnection(dbLocation);

            InitDatabase();
        }

        private void InitDatabase()
        {
            Task.Run(async () =>
            {
                await Connection!.CreateTableAsync<Feed>()
                    .ConfigureAwait(false);
            });
        }

        internal async Task<List<Feed>> GetFeeds()
        {
            var feedQuery = await Connection.QueryAsync<Feed>("SELECT [Id], [Name], [Url], [Category] FROM [Feeds]");
            return feedQuery ?? new();
        }
        internal async Task<Feed?> GetFeed(Guid feedId)
        {
            var feedQuery = await Connection.QueryAsync<Feed>("SELECT [Id], [Name], [Url], [Category] FROM [Feeds] WHERE [Id] = ?", feedId);
            return feedQuery.FirstOrDefault();
        }

        internal async Task<bool> FeedExistsByUrl(string url)
        {
            var query = await Connection.QueryAsync<Feed>("SELECT 1 FROM [Feeds] WHERE [Url] = ?", url);
            return query.Any();
        }

        internal async Task<bool> FeedExistsById(Guid Id)
        {
            var query = await Connection.QueryAsync<Feed>("SELECT 1 FROM [Feeds] WHERE [Id] = ?", Id);
            return query.Any();
        }

        internal async Task<bool> AddFeed(string name, string url)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("Name or Url is empty");
            }

            var insert = await Connection.InsertAsync(new Feed
            {
                Name = name,
                Url = url,
                Id = Guid.NewGuid(),
            });

            return insert == 1;
        }

        internal async Task<bool> RemoveFeedById(Guid Id)
        {
            var delete = await Connection.DeleteAsync<Feed>(Id);

            return delete == 1;
        }

        internal async Task<RSSFeed?> LoadRssFeed(Guid feedId)
        {
            // No need to check if it exists here, GetFeed will return null if nothing is found.
            var feed = await GetFeed(feedId);
            if (feed == null)
            {
                return null;
            }

            return LoadRssFeed(feed.Url);
        }

        private RSSFeed LoadRssFeed(string feedUrl)
        {
            try
            {
                var reader = XmlReader.Create(feedUrl);
                var loadedFeed = SyndicationFeed.Load(reader);

                var feed = new RSSFeed
                {
                    Description = loadedFeed.Description.Text,
                    Items = new(),
                    Title = loadedFeed.Title.Text,
                    LastUpdate = loadedFeed.LastUpdatedTime.LocalDateTime
                };

                foreach (SyndicationItem item in loadedFeed.Items)
                {
                    var feedItem = new FeedItem
                    {
                        Categories = item.Categories.Select(x => x.Name).ToList(),
                        Id = item.Id, // Naively assume the Id is the link to the article
                        Published = item.PublishDate.LocalDateTime,
                        Updated = item.LastUpdatedTime.LocalDateTime,
                        Summary = item.Summary.Text,
                        Title = item.Title.Text
                    };

                    /*
                    foreach (SyndicationElementExtension ext in item.ElementExtensions)
                    {
                        //if (ext.GetObject<XElement>().Name.LocalName == "encoded")
                        //{
                        var content = ext.GetObject<XElement>().Value;
                        //}
                    }
                    */

                    feed.Categories.AddRange(feedItem.Categories.Except(feed.Categories));
                    feed.Items.Add(feedItem);
                }

                return feed;
            }
            catch
            {
                throw;
            }
        }
    }


    public class RSSFeed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<FeedItem> Items { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<string> Categories { get; private set; } = new();
    }

    public class FeedItem
    {
        public List<string> Categories { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public DateTime Updated { get; set; }
        public string Summary { get; set; }
        /// <summary>
        /// Naively assumed to be the Url to the feed item article
        /// </summary>
        public string Id { get; set; }
    }
}
