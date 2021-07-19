using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Model
{
    public partial class Hash
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public DateTime? Expireat { get; set; }
        public int Updatecount { get; set; }
    }
}
