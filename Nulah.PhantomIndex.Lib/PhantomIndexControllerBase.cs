using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib
{
    public abstract class PhantomIndexControllerBase
    {
        public readonly PhantomIndexManager PhantomIndexManager;

        public PhantomIndexControllerBase(PhantomIndexManager phantomIndexManager)
        {
            PhantomIndexManager = phantomIndexManager;
        }

        /// <summary>
        /// Ensures that the database has a connection, and in any derived classes, required tables are created
        /// </summary>
        /// <exception cref="Exception"></exception>
        internal virtual void Init()
        {
            if (PhantomIndexManager.Connection == null)
            {
                throw new Exception("Connection has not been created");
            }
        }
    }
}
