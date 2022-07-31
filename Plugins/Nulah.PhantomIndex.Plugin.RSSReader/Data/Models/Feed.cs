using SQLite;
using System;

namespace Nulah.PhantomIndex.Plugin.RSSReader.Data.Models
{
    [Table("Feeds")]
    public class Feed
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [Unique]
        public string Url { get; set; }
        public string Name { get; set; }
        [Indexed]
        public string Category { get; set; }
    }
}
