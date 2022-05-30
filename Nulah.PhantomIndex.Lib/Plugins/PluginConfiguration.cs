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
        /// Internal use only to ensure values are only set before initialisation
        /// </summary>
        /// <param name="pluginDataLocation"></param>
        internal PluginConfiguration(string pluginDataLocation)
        {
            PluginDataLocation = pluginDataLocation;
        }
    }
}
