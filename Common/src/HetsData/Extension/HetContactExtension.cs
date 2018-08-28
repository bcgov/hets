using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetContact
    {
        [NotMapped]
        public int Id
        {
            get => ContactId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                ContactId = value;
            }
        }        
    }
}
