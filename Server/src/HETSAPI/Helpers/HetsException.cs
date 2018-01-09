using System;
using System.Runtime.Serialization;

namespace HETSAPI.Helpers
{
    /// <inheritdoc />
    /// <summary>
    /// Custom Hets Exception Class
    /// </summary>
    [Serializable()]
    public class HetsException : Exception
    {
        private readonly string _method;

        /// <summary>
        /// Source of theexception
        /// </summary>
        public string SourceMethod => _method;

        /// <summary>
        /// Hets Exception Constructor
        /// </summary>
        public HetsException()
        {
        }

        /// <summary>
        /// Hets Exception Constructor
        /// </summary>
        /// <param name="message"></param>
        public HetsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Exception Serializer
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected HetsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Hets Exception Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public HetsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public HetsException(string message, Exception innerException, string method) : base(message, innerException)
        {
            _method = method;
        }
    }
}
