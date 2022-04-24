using Nulah.PhantomIndex.Lib.Plugins;
using System.ComponentModel.Composition;

namespace Nulah.PhantomIndex.Lib
{
    public sealed class PhantomIndexManager
    {
        public readonly DatabaseManager Database;
        private readonly PluginManager _pluginManager;

        //[Export(PluginConstants.ApplicationDataLocationContractName)]
        public string? ApplicationLocation { get; private set; }

        /// <summary>
        /// Explicitly exported types to avoid annotation exports from Lib.
        /// </summary>
        // While some [Export] attributes may work (such as fields), others will fail to be imported,
        // such as properties. Why is unknown but uncomment the export attribute above and attempt to [Import] it in a plugin to see.
        private Dictionary<string, object> _exportContracts = new Dictionary<string, object>();

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
            _exportContracts.Add(PluginConstants.ApplicationDataLocationContractName, applicationLocalLocation);
        }

        /// <summary>
        /// Sets the user plugin location, and creates the directory if it does not exist
        /// </summary>
        /// <param name="localPluginDirectory"></param>
        public void SetLocalPluginLocation(string localPluginDirectory)
        {
            _pluginManager.UserPluginLocation = localPluginDirectory;
            Directory.CreateDirectory(localPluginDirectory);
            _exportContracts.Add(PluginConstants.UserPluginLocationContractName, localPluginDirectory);
        }

        public List<IPlugin> GetPlugins()
        {
            _pluginManager.DiscoverPlugins(_exportContracts);

            return _pluginManager.Plugins
                .Select(x => SetPluginSelfInstance(x.Value))
                .ToList();
        }

        /// <summary>
        /// Sets the static Instance field to the plugin itself
        /// <para>
        /// A plugin will only have a static reference to itself if a field matching
        /// <code>static IPlugin Instance;</code>
        /// exists by name and signature, irrespective of scope.
        /// </para>
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        private IPlugin SetPluginSelfInstance(IPlugin plugin)
        {
            // Honestly this is a bit of a hack, but for windowless plugins
            // (ones that use the main window and live in the NulahNavigation frame),
            // they may wish to keep plugin information within the plugin class itself.
            // Lacking a means of DI will at least allow them a reference to the plugin instance itself.

            // Could this be better? Yes.
            // Does it work? Yes.
            // Will it cause me problems later when I want more complex navigation? Maybe!
            var t = plugin.GetType();
            var pi = t.GetField("Instance");

            if (pi != null)
            {
                pi.SetValue(plugin, plugin);
            }

            return plugin;
        }
    }
}
