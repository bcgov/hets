using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetLog
    {
        public string Exception { get; set; }
        public int? Level { get; set; }
        public string LogEvent { get; set; }
        public string MachineName { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string PropsTest { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
