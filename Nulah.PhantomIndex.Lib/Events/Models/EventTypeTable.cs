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
    }
}
