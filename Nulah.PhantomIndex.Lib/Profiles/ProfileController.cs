using Nulah.PhantomIndex.Lib.Events.Models;
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
    public class ProfileController : DatabaseControllerBase
    {
        internal ProfileController(DatabaseManager databaseManager)
            : base(databaseManager)
        {
        }

        internal string ProfileTableName;

        internal override void Init()
        {
            base.Init();

            // Create tables required for this controller
            Task.Run(() => DatabaseManager.Connection
                !.CreateTableAsync<ProfileTable>()
                .ConfigureAwait(false));

            ProfileTableName = ((TableAttribute)typeof(ProfileTable).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(new TableAttribute("Profile"))).Name;
        }

        /// <summary>
        /// Creates a profile with the given details.
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="displayFirstname"></param>
        /// <param name="pronouns"></param>
        /// <param name="displayLastName"></param>
        /// <param name="imageBlob"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Thrown on any failure</exception>
        public async Task<ProfileTable> Create(string profileName, string displayFirstname, string pronouns, string? displayLastName = null, byte[]? imageBlob = null)
        {
            var newProfile = new ProfileTable
            {
                DisplayFirstName = displayFirstname,
                DisplayLastName = displayLastName,
                Id = Guid.NewGuid(),
                Name = profileName,
                Pronouns = pronouns,
                CreatedUtc = DateTime.UtcNow
            };

            // Get the event type for Created events
            var createdEventType = await DatabaseManager.Events.GetDefaultEventType(DefaultEventType.Created);

            var profileCreated = await DatabaseManager.Connection
                !.InsertAsync(newProfile)
                .ConfigureAwait(false);

            if (profileCreated == 1)
            {
                if (imageBlob != null)
                {
                    ImageResourceTable profileImage = await DatabaseManager.Images
                        .SaveImageForResource(newProfile.Id, imageBlob, nameof(ProfileTable))
                        .ConfigureAwait(false);
                }

                // Create a Created event to create an event that indicates the creation
                // :V
                await DatabaseManager.Events.CreateEvent(DateTime.UtcNow, newProfile.Id, createdEventType.Id);

                return newProfile;
            }

            // TODO: flesh this out to something more meaningful instead of a raw exception on failure
            throw new Exception($"Failed to create {nameof(ProfileTable)}");
        }

        /// <summary>
        /// Returns a <see cref="Profile"/> by <paramref name="profileId"/>. If no <see cref="Profile"/> is found, null is returned.
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public async Task<Profile?> GetProfile(Guid profileId)
        {
            var selectQuery = $@"SELECT
	                Profile.[{nameof(ProfileTable.Id)}] AS {nameof(Profile.Id)},
	                Profile.[{nameof(ProfileTable.Name)}] AS {nameof(Profile.Name)},
	                Profile.[{nameof(ProfileTable.DisplayFirstName)}] AS {nameof(Profile.DisplayFirstName)},
	                Profile.[{nameof(ProfileTable.DisplayLastName)}] AS {nameof(Profile.DisplayLastName)},
	                Profile.[{nameof(ProfileTable.Pronouns)}] AS {nameof(Profile.Pronouns)},
                    Profile.[{nameof(ProfileTable.CreatedUtc)}] AS {nameof(Profile.CreatedUtc)},
	                Image.[{nameof(ImageResourceTable.ImageBlob)}] AS {nameof(Profile.ImageBlob)}
                FROM 
                    [{ProfileTableName}] AS Profile
                LEFT JOIN [{DatabaseManager.Images.ImageResourceLinkTableName}] AS IR
	                ON IR.[ResourceId] = Profile.[Id]
                LEFT JOIN [{DatabaseManager.Images.ImageTableName}] AS Image
	                ON IR.[ImageId] = Image.[Id]
                WHERE
                    Profile.[Id] = ?;
                ";

            var profile = await DatabaseManager.Connection!.QueryAsync<Profile>(selectQuery, new[] { profileId.ToString() });

            return profile.FirstOrDefault();
        }

        /// <summary>
        /// Returns the total number of <see cref="Profile"/>s that exist
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetProfileCount()
        {
            return await DatabaseManager.Connection!.Table<ProfileTable>()
                .CountAsync();
        }

        /// <summary>
        /// Returns a list of <see cref="Profile"/> by the given <paramref name="pageSize"/> and <paramref name="pageStart"/>
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageStart"></param>
        /// <returns></returns>
        public async Task<List<Profile>> GetProfiles(int pageSize = 25, int pageStart = 0)
        {
            return await GetProfilesAsync(pageSize, pageStart);
            //.ConfigureAwait(false);
        }

        /// <summary>
        /// Returns all profiles limited by the given parameters
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageStart"></param>
        /// <returns></returns>
        private async Task<List<Profile>> GetProfilesAsync(int pageSize = 25, int pageStart = 0)
        {
            var selectQuery = $@"SELECT
	                Profile.[{nameof(ProfileTable.Id)}] AS {nameof(Profile.Id)},
	                Profile.[{nameof(ProfileTable.Name)}]AS {nameof(Profile.Name)},
	                Profile.[{nameof(ProfileTable.DisplayFirstName)}]AS {nameof(Profile.DisplayFirstName)},
	                Profile.[{nameof(ProfileTable.DisplayLastName)}]AS {nameof(Profile.DisplayLastName)},
	                Profile.[{nameof(ProfileTable.Pronouns)}]AS {nameof(Profile.Pronouns)},
                    Profile.[{nameof(ProfileTable.CreatedUtc)}] AS {nameof(Profile.CreatedUtc)},
	                Image.[{nameof(ImageResourceTable.ImageBlob)}] AS {nameof(Profile.ImageBlob)}
                FROM 
                    [{ProfileTableName}] AS Profile
                LEFT JOIN [{DatabaseManager.Images.ImageResourceLinkTableName}] AS IR
	                ON IR.[ResourceId] = Profile.[Id]
                LEFT JOIN [{DatabaseManager.Images.ImageTableName}] AS Image
	                ON IR.[ImageId] = Image.[Id]
                LIMIT 
                    {pageStart},{pageSize + pageStart};
                ";

            var profiles = await DatabaseManager.Connection!.QueryAsync<Profile>(selectQuery);

            return profiles;
        }
    }
}
