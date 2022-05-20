using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Plugins
{
    public sealed class PluginConstants
    {
        /// <summary>
        /// Data contract name to be used within <see cref="ImportAttribute"/> to obtain the current application data location.
        /// </summary>
        public const string ApplicationDataLocationContractName = "AppDataLocation";
        /// <summary>
        /// Data contract name to be used within <see cref="ImportAttribute"/> to obtain the current user plugin data location.
        /// </summary>
        public const string UserPluginLocationContractName = "UserPluginLocation";
    }
}
