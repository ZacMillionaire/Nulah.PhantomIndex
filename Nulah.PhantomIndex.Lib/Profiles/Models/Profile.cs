using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Profiles.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayFirstName { get; set; } = string.Empty;
        public string? DisplayLastName { get; set; }
        public string Pronouns { get; set; } = string.Empty;
        public byte[]? ImageBlob { get; set; }
        public DateTime CreatedUtc { get; set; }

    }
}
