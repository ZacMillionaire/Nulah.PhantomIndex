using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Events.Models
{
    [Table("Events")]
    public class EventTable
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [Indexed]
        public Guid ProfileId { get; set; }
        [Indexed]
        public Guid EventTypeId { get; set; }
        public DateTime EventTimeUTC { get; set; }
        public string? TextContent { get; set; }
        public int? NumericContent { get; set; }
        public DateTime? DateTimeContent { get; set; }
        public byte[]? BlobContent { get; set; }
    }
}
