using Nulah.PhantomIndex.Lib.Profiles.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Profiles
{
    public class ProfileController
    {
        private readonly PhantomIndexDatabase _database;

        internal ProfileController(PhantomIndexDatabase database)
        {
            _database = database;
        }

        internal void Init()
        {
            if (_database.Connection == null)
            {
                throw new Exception("Connection has not been created");
            }

            Task.Run(() => _database.Connection.CreateTableAsync<Profile>().ConfigureAwait(false));
        }

        public async Task<Profile> Create(string profileName, string displayFirstname, string pronouns, string? displayLastName = null, byte[]? imageBlob = null)
        {
            if (_database.Connection == null)
            {
                throw new Exception("Connection has not been created");
            }

            var newProfile = new Profile
            {
                DisplayFirstName = displayFirstname,
                DisplayLastName = displayLastName,
                Id = Guid.NewGuid(),
                Name = profileName,
                Pronouns = pronouns
            };

            var profileCreated = await _database.Connection.InsertAsync(newProfile);

            if (profileCreated == 1)
            {
                _database.
                return newProfile;
            }

            return new();
        }
    }
}
