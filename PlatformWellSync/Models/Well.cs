using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformWellSync.Models
{
    internal class Well
    {
        public int Id { get; set; }

        public int PlatformId { get; set; }

        public string? WellName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Platform? Platform { get; set; }
    }
}
