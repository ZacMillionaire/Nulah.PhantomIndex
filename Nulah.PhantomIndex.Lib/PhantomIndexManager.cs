using Nulah.PhantomIndex.Core.Controls;
using Nulah.PhantomIndex.Lib.Plugins;
using System.ComponentModel.Composition;
using System.Reflection;

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

        /// <summary>
        /// Finds and configures all plugins found, and returns them ready for <see cref="NulahPlugin.OnPluginInitialise"/>
        /// </summary>
        /// <returns></returns>
        public List<NulahPlugin> GetPlugins()
        {
            _pluginManager.DiscoverPlugins();

            return _pluginManager.Plugins
                .Select(x =>
                {
                    var plugin = SetPluginDetails(x.Value);
                    plugin.OnPluginInitialise();
                    return plugin;
                })
                .ToList();
        }

        private NulahPlugin SetPluginDetails(NulahPlugin plugin)
        {
            var config = new PluginConfiguration(Path.Combine(_pluginManager.UserPluginLocation!, plugin.GetType().Name),
                // It shouldn't be possible for the plugin assembly location to be null
                new DirectoryInfo(plugin.GetType().Assembly.Location).Parent!.FullName);
            Directory.CreateDirectory(config.PluginDataLocation);

            plugin.Details = config;

            return plugin;
        }
    }
}
