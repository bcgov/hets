using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Model
{
    public partial class Lock
    {
        public string Resource { get; set; }
        public int Updatecount { get; set; }
        public DateTime? Acquired { get; set; }
    }
}
