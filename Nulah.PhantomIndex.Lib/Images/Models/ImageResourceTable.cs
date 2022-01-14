using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Lib.Images.Models
{
    [Table("Images")]
    public class ImageResourceTable
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mimetype { get; set; }
        public long Filesize { get; set; }
        public byte[] ImageBlob { get; set; }
    }
}
