using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetPerson
    {
        [NotMapped]
        public int Id
        {
            get => PersonId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                PersonId = value;
            }
        }
    }
}
