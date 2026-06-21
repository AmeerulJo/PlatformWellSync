using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformWellSync.Models
{
    internal class PlatformDto
    {
        public int Id { get; set; }

        public string? PlatformName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public List<WellDto> Wells { get; set; } = new();
    }
}
