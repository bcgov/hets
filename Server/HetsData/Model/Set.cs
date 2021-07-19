using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Model
{
    public partial class Set
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public double Score { get; set; }
        public string Value { get; set; }
        public DateTime? Expireat { get; set; }
        public int Updatecount { get; set; }
    }
}
