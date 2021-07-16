using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Model
{
    public partial class Job
    {
        public Job()
        {
            Jobparameters = new HashSet<Jobparameter>();
            States = new HashSet<State>();
        }

        public long Id { get; set; }
        public long? Stateid { get; set; }
        public string Statename { get; set; }
        public string Invocationdata { get; set; }
        public string Arguments { get; set; }
        public DateTime Createdat { get; set; }
        public DateTime? Expireat { get; set; }
        public int Updatecount { get; set; }

        public virtual ICollection<Jobparameter> Jobparameters { get; set; }
        public virtual ICollection<State> States { get; set; }
    }
}
