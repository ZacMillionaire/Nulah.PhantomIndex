using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Categories.Models
{
    [Table("Categories")]
    public class CategoryTable
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        [NotNull]
        public string Type { get; set; }
    }
}
