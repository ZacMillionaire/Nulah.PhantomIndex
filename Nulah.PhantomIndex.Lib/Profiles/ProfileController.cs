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

        internal string ProfileTableName;

        internal override void Init()
        {
            base.Init();

            // Create tables required for this controller
            Task.Run(() => PhantomIndexManager.Connection
                !.CreateTableAsync<ProfileTable>()
                .ConfigureAwait(false));

            ProfileTableName = ((TableAttribute)typeof(ProfileTable).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(new TableAttribute("Profile"))).Name;
        }

        public async Task<ProfileTable> Create(string profileName, string displayFirstname, string pronouns, string? displayLastName = null, byte[]? imageBlob = null)
        {
            var newProfile = new ProfileTable
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
                    ImageResourceTable profileImage = await PhantomIndexManager.Images
                        .SaveImageForResource(newProfile.Id, imageBlob, nameof(ProfileTable))
                        .ConfigureAwait(false);
                }
                return newProfile;
            }

            // TODO: flesh this out to something more meaningful instead of a raw exception on failure
            throw new Exception($"Failed to create {nameof(ProfileTable)}");
        }

        public async Task<List<Profile>> GetProfiles(int pageSize = 25, int pageStart = 0)
        {
            return await GetProfilesAsync(pageSize, pageStart);
            //.ConfigureAwait(false);
        }

        private async Task<List<Profile>> GetProfilesAsync(int pageSize = 25, int pageStart = 0)
        {
            var selectQuery = $@"SELECT
	                Profile.[{nameof(ProfileTable.Id)}] AS {nameof(Profile.Id)},
	                Profile.[{nameof(ProfileTable.Name)}]AS {nameof(Profile.Name)},
	                Profile.[{nameof(ProfileTable.DisplayFirstName)}]AS {nameof(Profile.DisplayFirstName)},
	                Profile.[{nameof(ProfileTable.DisplayLastName)}]AS {nameof(Profile.DisplayLastName)},
	                Profile.[{nameof(ProfileTable.Pronouns)}]AS {nameof(Profile.Pronouns)},
	                Image.[{nameof(ImageResourceTable.ImageBlob)}] AS {nameof(Profile.ImageBlob)}
                FROM 
                    [{ProfileTableName}] AS Profile
                LEFT JOIN [{PhantomIndexManager.Images.ImageResourceLinkTableName}] AS IR
	                ON IR.[ResourceId] = Profile.[Id]
                LEFT JOIN [{PhantomIndexManager.Images.ImageTableName}] AS Image
	                ON IR.[ImageId] = Image.[Id]
                LIMIT 
                    {pageStart},{pageSize + pageStart};
                ";

            var profiles = await PhantomIndexManager.Connection!.QueryAsync<Profile>(selectQuery);

            return profiles;
        }
    }
}
