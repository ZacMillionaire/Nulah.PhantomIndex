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
        private PluginManager _pluginManager;

        public PhantomIndexManager()
        {
            Database = new DatabaseManager();
            _pluginManager = new PluginManager();
        }

        public void SetPluginLocation(string pluginDirectory)
        {
            _pluginManager.Location = pluginDirectory;
        }
    }
}
