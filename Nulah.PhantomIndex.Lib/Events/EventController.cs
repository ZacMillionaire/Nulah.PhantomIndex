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

        internal override void Init()
        {
            base.Init();

            // Create tables required for this controller
            Task.Run(() =>
            {
                PhantomIndexManager.Connection!.CreateTableAsync<EventTable>().ConfigureAwait(false);
                PhantomIndexManager.Connection!.CreateTableAsync<EventTypeTable>().ConfigureAwait(false);
            });

            EventTableName = ((TableAttribute)typeof(EventTable).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(new TableAttribute("Event"))).Name;
            EventTypeTableName = ((TableAttribute)typeof(EventTypeTable).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault(new TableAttribute("EventType"))).Name;
        }

        public Event CreateEvent()
        {
            throw new NotImplementedException();
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
            // Certain types will not have a .FullName, which is required for when we convert the event types...type back
            // to its original Type using Type.GetType(string typeName)
            if (typeof(T).FullName == null)
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
                Name = name,
                Type = typeof(T)!.FullName
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
