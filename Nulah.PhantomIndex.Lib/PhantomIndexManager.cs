using Nulah.PhantomIndex.Lib.Events;
using Nulah.PhantomIndex.Lib.Images;
using Nulah.PhantomIndex.Lib.Plugins;
using Nulah.PhantomIndex.Lib.Profiles;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib
{
    public sealed class PhantomIndexManager
    {
        public readonly DatabaseManager Database;
        private readonly PluginManager _pluginManager;

        public string? ApplicationLocation { get; private set; }

        public PhantomIndexManager()
        {
            Database = new DatabaseManager();
            _pluginManager = new PluginManager();
        }

        /// <summary>
        /// Sets the local application location, and creates the location if it does not exist
        /// </summary>
        /// <param name="applicationLocalLocation"></param>
        public void SetApplicationLocation(string applicationLocalLocation)
        {
            ApplicationLocation = applicationLocalLocation;
            Directory.CreateDirectory(ApplicationLocation);
        }

        /// <summary>
        /// Sets the user plugin location, and creates the directory if it does not exist
        /// </summary>
        /// <param name="localPluginDirectory"></param>
        public void SetLocalPluginLocation(string localPluginDirectory)
        {
            _pluginManager.UserPluginLocation = localPluginDirectory;
            Directory.CreateDirectory(localPluginDirectory);
        }

        public List<IPlugin> GetPlugins()
        {
            _pluginManager.DiscoverPlugins();

            return _pluginManager.Plugins
                .Select(x => x.Value)
                .ToList();
        }
    }
}
