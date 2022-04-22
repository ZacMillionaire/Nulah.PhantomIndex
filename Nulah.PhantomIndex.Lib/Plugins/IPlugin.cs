using Nulah.PhantomIndex.Core.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Plugins
{
    public interface IPlugin
    {
        public string Name { get; set; }
        public Dictionary<string, (FontIcon?, string)> Pages { get; set; }
    }
}
