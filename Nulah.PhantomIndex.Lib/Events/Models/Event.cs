using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Events.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public Guid ProfileId { get; set; }
        public Guid EventType { get; set; }
        public string Content { get; set; }
    }
}
