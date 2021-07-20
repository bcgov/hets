using System;
using System.Runtime.Serialization;

namespace HetsData.Entities
{
    [Serializable]
    public class HetsDbConcurrencyException : Exception
    {
        public HetsDbConcurrencyException()
        {
        }

        public HetsDbConcurrencyException(string message) : base(message)
        {
        }

        public HetsDbConcurrencyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected HetsDbConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}