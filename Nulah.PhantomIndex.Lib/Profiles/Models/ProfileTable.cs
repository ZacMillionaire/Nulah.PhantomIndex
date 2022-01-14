using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Profiles.Models
{
    [Table("Profiles")]
    public class ProfileTable
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [NotNull]
        public string Name { get; set; } = string.Empty;
        [NotNull]
        public string DisplayFirstName { get; set; } = string.Empty;
        public string? DisplayLastName { get; set; }
        [NotNull]
        public string Pronouns { get; set; } = string.Empty;
    }
}
