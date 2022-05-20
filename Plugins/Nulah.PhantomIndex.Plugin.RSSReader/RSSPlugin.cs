using Nulah.PhantomIndex.Core.Controls;
using Nulah.PhantomIndex.Lib;
using Nulah.PhantomIndex.Lib.Plugins;
using Nulah.PhantomIndex.Plugin.RSSReader.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Nulah.PhantomIndex.Plugin.RSSReader
{
    [Export(typeof(IPlugin))]
    internal class RSSPlugin : IPlugin
    {
        // In lieu of being able to define statics on interfaces
        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/static-abstract-interface-methods
        internal static RSSPlugin Instance;
        internal static NulahNavigation WindowNavigation;

        public string Name => "RSS";

        public FontIcon Icon => FontIcon.Wifi;

        public List<PluginMenuItem> Pages => new()
        {
            new PluginMenuItem("Feeds", "Pages/Feeds/FeedList")
        };

        public readonly string PluginDataLocation;
        public readonly RSSFeeder Feeder;

        [ImportingConstructor]
        public RSSPlugin([Import(PluginConstants.UserPluginLocationContractName)] string pluginAppDataLocation)
        {
            PluginDataLocation = Path.Combine(pluginAppDataLocation, Name);
            Directory.CreateDirectory(PluginDataLocation);

            Feeder = new RSSFeeder(Path.Combine(PluginDataLocation, $"{Name}.db"));
        }
    }
}
