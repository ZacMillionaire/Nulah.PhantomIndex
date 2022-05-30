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
    [Export(typeof(NulahPlugin))]
    internal class RSSPlugin : NulahPlugin
    {
        internal static RSSPlugin Instance = null;

        public RSSFeeder? Feeder { get; private set; }

        public RSSPlugin()
        {
            Instance = this;
        }

        public override Task OnPluginInitialise()
        {

            Name = "RSS";
            Icon = FontIcon.Wifi;

            Pages.Add(new PluginMenuItem("Feeds", "Pages/Feeds/FeedList"));

            Feeder = new RSSFeeder(Path.Combine(Details.PluginDataLocation, $"{Name}.db"));

            return Task.CompletedTask;
        }

        public override Task OnApplicationClose()
        {
            throw new NotImplementedException();
        }
    }
}
