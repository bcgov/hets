using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class Contact : HetContact
    {
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
