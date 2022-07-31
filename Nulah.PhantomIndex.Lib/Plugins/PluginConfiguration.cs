using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Plugins
{
    public class PluginConfiguration
    {
        /// <summary>
        /// %appdata% location for the plugin. Guaranteed to exist.
        /// </summary>
        public string PluginDataLocation;

        /// <summary>
        /// Location of the plugin assemblies. Guaranteed to exist.
        /// </summary>
        public string PluginLocation;

        /// <summary>
        /// Internal use only to ensure values are only set before initialisation
        /// </summary>
        /// <param name="pluginDataLocation"></param>
        /// <paramref name="pluginAssemblyLocation"/>
        internal PluginConfiguration(string pluginDataLocation, string pluginAssemblyLocation)
        {
            PluginDataLocation = pluginDataLocation;
            PluginLocation = pluginAssemblyLocation;
        }
    }
}
