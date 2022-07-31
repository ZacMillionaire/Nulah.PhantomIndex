using Nulah.PhantomIndex.Core.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Plugins
{
    /// <summary>
    /// Base class for Plugins. <see cref="Name"/>, <see cref="Icon"/> should be set within <see cref="OnPluginInitialise"/>, 
    /// and pages for your plugin should be added to <see cref="Pages"/>.
    /// </summary>
    public abstract class NulahPlugin
    {
        public PluginConfiguration Details { get; internal set; }

        public List<PluginMenuItem> Pages = new();
        public string Name { get; set; }
        public FontIcon Icon { get; set; }
        /// <summary>
        /// Path to the "index" page of the plugin, without extensions
        /// </summary>
        public string EntryPage { get; set; }

        public abstract Task OnPluginInitialise();
        public abstract Task OnApplicationClose();
    }
}
