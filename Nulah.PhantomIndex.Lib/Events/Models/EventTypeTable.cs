using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Events.Models
{
    [Table("EventTypes")]
    public class EventTypeTable
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [NotNull]
        [Unique]
        public string Name { get; set; }
        /// <summary>
        /// C# DataType
        /// </summary>
        [NotNull]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// String format to use when displaying the value of the attached Event
        /// </summary>
        public string? StringFormat { get; set; }
        /// <summary>
        /// Indicates if the event type is required by the application (read only as far as logic should be concerned)
        /// </summary>
        public bool IsReadOnly { get; set; }
    }
}
