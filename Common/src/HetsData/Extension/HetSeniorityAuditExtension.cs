using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetSeniorityAudit
    {
        [NotMapped]
        public int Id
        {
            get => SeniorityAuditId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                SeniorityAuditId = value;
            }
        }
    }
}
