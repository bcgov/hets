using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetNote
    {
        [NotMapped]
        public int Id
        {
            get => NoteId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                NoteId = value;
            }
        }
    }
}
