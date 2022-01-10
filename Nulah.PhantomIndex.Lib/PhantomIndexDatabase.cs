using Nulah.PhantomIndex.Lib.Images;
using Nulah.PhantomIndex.Lib.Profiles;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib
{
    public sealed class PhantomIndexDatabase
    {
        internal SQLiteAsyncConnection? Connection;
        public ProfileController Profiles;
        public ImageController Images;

        public PhantomIndexDatabase()
        {
            Profiles = new ProfileController(this);
            Profiles = new ImageController(this);
        }

        public void SetConnection(string connectionString)
        {
            Connection = new SQLiteAsyncConnection(connectionString);

            Profiles.Init();
        }
    }
}
