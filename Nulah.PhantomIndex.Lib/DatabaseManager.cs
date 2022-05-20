using Nulah.PhantomIndex.Lib.Events;
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
    public class DatabaseManager
    {

        public ProfileController Profiles;
        public ImageController Images;
        public EventController Events;
        public string DatabaseLocation;
        internal SQLiteAsyncConnection? Connection;

        public DatabaseManager()
        {
            Profiles = new ProfileController(this);
            Images = new ImageController(this);
            Events = new EventController(this);
        }

        public void SetConnection(string connectionString)
        {
            DatabaseLocation = connectionString;

            Connection = new SQLiteAsyncConnection(connectionString);

            Profiles.Init();
            Images.Init();
            Events.Init();
        }
    }
}
