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

        public EventType CreateEventType<T>(string name)
        {
            var newEventType = new EventTypeTable
            {
                Name = name,
                Type = typeof(T).Name
            };

            var a = Type.GetType(newEventType.Type);

            throw new NotImplementedException();
        }
    }
}
