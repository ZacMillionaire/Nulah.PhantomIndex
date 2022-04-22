using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Plugins
{
    internal class PluginManager
    {
        /// <summary>
        /// Directory to the %appdata% location for custom user plugins
        /// </summary>
        public string? UserPluginLocation { get; internal set; }

        [ImportMany(typeof(IPlugin))]
        internal List<Lazy<IPlugin, IDictionary<string, object>>> Plugins = new();

        /// <summary>
        /// Finds all plugins in the application plugin folder, and the users %localAppData% location
        /// </summary>
        internal void DiscoverPlugins()
        {
            var catalog = new AggregateCatalog();

            // Add the default discovery location for plugins, which is the executing app directory/plugins
            var defaultPluginStore = Path.Combine(
               new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!,
                "Plugins"
            );

            // Get all top level folders within it (do not search sub directories)
            GetPluginDirectories(defaultPluginStore, catalog);
            // Do the same for the user plugin directory, which should be %localappdata%/Nulah.PhantomIndex.WPF/Plugins
            GetPluginDirectories(UserPluginLocation, catalog);

            var _container = new CompositionContainer(catalog);
            _container.SatisfyImportsOnce(this);
        }

        /// <summary>
        /// Finds all plugins under the given directory, and adds to the aggregate catalog
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <param name="catalog"></param>
        private void GetPluginDirectories(string? baseDirectory, AggregateCatalog catalog)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory) == false)
            {
                var basePluginDirectory = Directory.GetDirectories(baseDirectory);
                foreach (string pluginFolder in basePluginDirectory)
                {
                    catalog.Catalogs.Add(new DirectoryCatalog(pluginFolder));
                }
            }
        }

    }
}
