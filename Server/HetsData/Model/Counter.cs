using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Model
{
    public partial class Counter
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public long Value { get; set; }
        public DateTime? Expireat { get; set; }
    }
}
