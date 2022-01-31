using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Events.Models
{
    public class EventType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// C# DataType
        /// </summary>
        public string Type { get; set; }
        public string StringFormat { get; set; }
        /// <summary>
        /// Indicates that the event type is required by the application and should be read only
        /// </summary>
        public bool IsReadOnly { get; set; }
    }
}
