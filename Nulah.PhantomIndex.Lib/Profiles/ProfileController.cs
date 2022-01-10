using Nulah.PhantomIndex.Lib.Images.Models;
using Nulah.PhantomIndex.Lib.Profiles.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Profiles
{
    public class ProfileController : PhantomIndexControllerBase
    {
        internal ProfileController(PhantomIndexManager phantomIndexManager)
            : base(phantomIndexManager)
        {
        }

        internal override void Init()
        {
            base.Init();

            // Create tables required for this controller
            Task.Run(() => PhantomIndexManager.Connection
                !.CreateTableAsync<Profile>()
                .ConfigureAwait(false));
        }

        public async Task<Profile> Create(string profileName, string displayFirstname, string pronouns, string? displayLastName = null, byte[]? imageBlob = null)
        {
            var newProfile = new Profile
            {
                DisplayFirstName = displayFirstname,
                DisplayLastName = displayLastName,
                Id = Guid.NewGuid(),
                Name = profileName,
                Pronouns = pronouns
            };

            var profileCreated = await PhantomIndexManager.Connection
                !.InsertAsync(newProfile)
                .ConfigureAwait(false);

            if (profileCreated == 1)
            {
                if (imageBlob != null)
                {
                    ImageResource profileImage = await PhantomIndexManager.Images
                        .SaveImageForResource(newProfile.Id, imageBlob, nameof(Profile))
                        .ConfigureAwait(false);
                }
                return newProfile;
            }

            // TODO: flesh this out to something more meaningful instead of a raw exception on failure
            throw new Exception($"Failed to create {nameof(Profile)}");
        }
    }
}
