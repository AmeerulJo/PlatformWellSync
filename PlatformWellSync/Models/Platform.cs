using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformWellSync.Models
{
    internal class Platform
    {
        public int Id { get; set; }

        public string? PlatformName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public ICollection<Well> Wells { get; set; } = new List<Well>();
    }
}
