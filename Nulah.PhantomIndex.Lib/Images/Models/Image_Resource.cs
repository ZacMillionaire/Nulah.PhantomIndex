using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Images.Models
{
    [Table("Image_Resource")]
    public class Image_Resource
    {
        [PrimaryKey]
        public Guid ImageId { get; set; }
        [PrimaryKey]
        public Guid ResourceId { get; set; }
    }
}
