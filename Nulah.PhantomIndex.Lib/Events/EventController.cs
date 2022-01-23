using Nulah.PhantomIndex.Lib.Events.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Events
{
    public class EventController : PhantomIndexControllerBase
    {
        internal EventController(PhantomIndexManager phantomIndexManager)
            : base(phantomIndexManager)
        {
        }

        internal string EventTableName;
        internal string EventTypeTableName;

        /// <summary>
        /// Assembly full name of the <see cref="DateTime"/> type for use with event type conversion
        /// </summary>
        private string _datetimeClassType = typeof(DateTime).FullName;

        internal override void Init()
        {
            base.Init();

            // Create tables required for this controller
            Task.Run(async () =>
            {
                await PhantomIndexManager.Connection!.CreateTableAsync<EventTable>().ConfigureAwait(false);
                await PhantomIndexManager.Connection!.CreateTableAsync<EventTypeTable>().ConfigureAwait(false);

                EventTableName = ((TableAttribute)typeof(EventTable).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(new TableAttribute("Event"))).Name;
                EventTypeTableName = ((TableAttribute)typeof(EventTypeTable).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(new TableAttribute("EventType"))).Name;


                await CreateEventType<DateTime>(DefaultEventType.Created.ToString()).ConfigureAwait(false);

                // Validate all required default event types exist
                foreach (var defaultEventType in Enum.GetValues<DefaultEventType>())
                {
                    var defaultEventExists = await GetEventTypeExistByName(defaultEventType.ToString()).ConfigureAwait(false);
                    if (defaultEventExists == false)
                    {
                        throw new Exception($"Failed to create default event type {defaultEventType}");
                    }
                }
            });
        }

        /// <summary>
        /// Creates and returns a <see cref="DateTimeEvent"/>, attached to the given profile and event type
        /// </summary>
        /// <param name="dateTimeUTC"></param>
        /// <param name="profileId"></param>
        /// <param name="eventTypeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DateTimeEvent> CreateEvent(DateTime dateTimeUTC, Guid profileId, Guid eventTypeId)
        {
            var newEvent = new EventTable
            {
                Id = Guid.NewGuid(),
                EventTypeId = eventTypeId,
                ProfileId = profileId,
                DateTimeContent = dateTimeUTC,
                EventTimeUTC = DateTime.UtcNow
            };

            var eventCreated = await PhantomIndexManager.Connection
                !.InsertAsync(newEvent)
                .ConfigureAwait(false);

            if (eventCreated == 1)
            {
                // Should probably null check here at some point
                return await GetDateTimeEvent(newEvent.Id);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a <see cref="DateTimeEvent"/> by <paramref name="eventId"/> if it exists
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<DateTimeEvent?> GetDateTimeEvent(Guid eventId)
        {
            var getEventQuery = $@"SELECT
	                [Id],
	                [ProfileId],
	                [EventTypeId],
	                [DateTimeContent]
                FROM
	                [Events] AS Events
                WHERE
	                Events.[Id] = ?";

            var dateTimeEvent = await PhantomIndexManager.Connection!.QueryAsync<EventTable>(getEventQuery, eventId);

            if (dateTimeEvent.Count == 1)
            {
                // DateTimeContent can be null within the database, but should not be null
                // for events created with a types of DateTime
                if (dateTimeEvent[0].DateTimeContent == null)
                {
                    throw new NotSupportedException("Unable to understand event: DateTimeContent was null");
                }

                return new DateTimeEvent
                {
                    Id = dateTimeEvent[0].Id,
                    ProfileId = dateTimeEvent[0].ProfileId,
                    EventTypeId = dateTimeEvent[0].EventTypeId,
                    EventTimeUTC = dateTimeEvent[0].EventTimeUTC,
                    Content = dateTimeEvent[0].DateTimeContent!.Value
                };
            }

            return null;
        }

        /// <summary>
        /// Creates a new event type of the given type <typeparamref name="T"/> and returns it.
        /// <para>
        /// Will not recreate the event if it already exists by name
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">Unique name for the type</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<EventType> CreateEventType<T>(string name)
        {
            var typeName = typeof(T).FullName;
            // Certain types will not have a .FullName, which is required for when we convert the event types...type back
            // to its original Type using Type.GetType(string typeName)
            if (string.IsNullOrWhiteSpace(typeName) == true)
            {
                throw new NotSupportedException($"{typeof(T).Name} is not supported - no FullName value found");
            }

            var exists = await GetEventTypeExistByName(name);

            if (exists == true)
            {
                // If the event type exists by name this cannot return null,
                return await GetEventTypeByName(name);
            }

            var newEventType = new EventTypeTable
            {
                Id = Guid.NewGuid(),
                Name = name,
                Type = typeName
            };

            var insert = await PhantomIndexManager.Connection!.InsertAsync(newEventType).ConfigureAwait(false);

            if (insert == 1)
            {
                // If we successfully inserted a row, this cannot be null
                return await GetEventTypeByName(name);
            }

            throw new Exception($"Failed to create event type {name} of type {typeof(T).FullName}");
        }

        /// <summary>
        /// Returns a default event type that must exist
        /// </summary>
        /// <param name="defaultEventType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<EventType> GetDefaultEventType(DefaultEventType defaultEventType)
        {
            var eventType = await GetEventTypeByName(defaultEventType.ToString());

            if (eventType == null)
            {
                throw new Exception($"No default type exists for {defaultEventType}");
            }

            return eventType;
        }


        // TODO: have fun getting all events from the database, then looking up each event type, getting its C# type,
        // then casting each event to its proper event class (eg DateTimeEvent), and hoping it all fucking works
        // TODO: need to add a datetime to events to track when they happened for timeline display
        // TODO: the ui for events will be lazy loaded
        public async Task<List<Event>> GetEventsForProfile(Guid profileId)
        {
            var profileEvents = new List<Event>();

            var eventsForProfile = $@"SELECT
	                E.Id,
                    E.ProfileId,
                    E.EventTypeId,
	                E.EventTimeUTC,
	                E.TextContent,
	                E.NumericContent,
	                E.DateTimeContent,
	                E.BlobContent,
	                ET.Type AS ClassType
                FROM 
	                [Events] AS E
                JOIN [EventTypes] AS ET
	                ON ET.[Id] = E.[EventTypeId]
                WHERE
	                E.[ProfileId] = ?";

            var profileEventQuery = await PhantomIndexManager.Connection!.QueryAsync<EventHolding>(eventsForProfile, profileId);

            foreach (EventHolding profileEvent in profileEventQuery)
            {
                if (profileEvent.ClassType == _datetimeClassType)
                {
                    profileEvents.Add(new DateTimeEvent
                    {
                        Content = profileEvent.DateTimeContent!.Value,
                        EventTypeId = profileEvent.EventTypeId,
                        EventTimeUTC = profileEvent.EventTimeUTC,
                        Id = profileEvent.Id,
                        ProfileId = profileId
                    });
                }
            }

            return profileEvents;
        }

        // TODO: temp class for mapping, should move or make a strong class later
        public class EventHolding
        {
            public Guid Id { get; set; }
            public Guid EventTypeId { get; set; }
            public DateTime EventTimeUTC { get; set; }
            public string? TextContent { get; set; }
            public int? NumericContent { get; set; }
            public DateTime? DateTimeContent { get; set; }
            public byte[]? BlobContent { get; set; }
            public string ClassType { get; set; }
        }

        /// <summary>
        /// Returns true if an event type exists by the given name
        /// </summary>
        /// <param name="eventTypeName"></param>
        /// <returns></returns>
        private async Task<bool> GetEventTypeExistByName(string eventTypeName)
        {
            var existsQuery = $@"SELECT 1
                FROM [{EventTypeTableName}]
                    AS EventTypeTable
                WHERE
                    EventTypeTable.[Name] = ?";

            var exists = await PhantomIndexManager.Connection!.ExecuteScalarAsync<bool>(existsQuery, new object[] { eventTypeName });

            return exists;
        }

        /// <summary>
        /// Returns the event by the given name, or null if it does not exist
        /// </summary>
        /// <param name="eventTypeName"></param>
        /// <returns></returns>
        private async Task<EventType?> GetEventTypeByName(string eventTypeName)
        {
            var eventTypeQuery = $@"SELECT 
	                [Id],
	                [Name],
	                [Type]
                FROM [EventTypes]
	                AS EventType
                WHERE EventType.[Name] = ?";

            var eventTypes = await PhantomIndexManager.Connection!.QueryAsync<EventTypeTable>(eventTypeQuery, eventTypeName);

            if (eventTypes.Count == 1)
            {
                return new EventType
                {
                    Id = eventTypes[0].Id,
                    Name = eventTypes[0].Name,
                    Type = eventTypes[0].Type,
                };
            }

            return null;
        }
    }
}
