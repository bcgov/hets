using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Model
{
    public partial class Jobparameter
    {
        public long Id { get; set; }
        public long Jobid { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Updatecount { get; set; }

        public virtual Job Job { get; set; }
    }
}
