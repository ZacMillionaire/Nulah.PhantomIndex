using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

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
        /// <param name="exportedValues">A dictionary of values to be imported into plugins. 
        /// This is equivalent to annotaing with <see cref="ExportAttribute"/> for values
        /// </param>
        internal void DiscoverPlugins(Dictionary<string, object> exportedValues)
        {
            var catalog = new AggregateCatalog();

            // Add the default discovery location for plugins, which is the executing app directory/plugins
            var defaultPluginStore = Path.Combine(
               new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!,
                "Plugins"
            );
            // Ensure the plugin directory exists by creating it
            Directory.CreateDirectory(defaultPluginStore);

            // Get all top level folders within it (do not search sub directories)
            GetPluginDirectories(defaultPluginStore, catalog);
            // Do the same for the user plugin directory, which should be %localappdata%/Nulah.PhantomIndex.WPF/Plugins
            GetPluginDirectories(UserPluginLocation, catalog);

            var _container = new CompositionContainer(catalog);

            // Add any values we want to be importable by plugins, certain values cannot be successfully exported via annotations
            // so we enable direct passing of values during discovery that we want available
            foreach (KeyValuePair<string, object> exportedValue in exportedValues)
            {
                // exported values are objects, but ComposeExportedValue won't correctly retrieve their runtime type
                // due to the override being <T>, which exports objects which...breaks anything trying to import
                // import a specific type.
                // Casting properly ensures that <T> resolves to the actual type.
                if (exportedValue.Value is string exportedString)
                {
                    _container.ComposeExportedValue(exportedValue.Key, exportedString);
                }
            }

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
