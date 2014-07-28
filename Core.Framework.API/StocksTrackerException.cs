using System;
using System.Runtime.Serialization;

namespace Core.Framework.API
{
    /// <summary>
    /// The base class for all exceptions thrown by the StocksTracker API implementations.
    /// </summary>
    [Serializable]
    public class StocksTrackerException : Exception
    {
        /// <see cref="System.Exception(SerializationInfo, StreamingContext)"></see>
        protected StocksTrackerException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }

        /// <see cref="System.Exception(String)"></see>
        public StocksTrackerException(String message) : base(message)
        {
        }

        /// <see cref="System.Exception(String, System.Exception)"></see>
        public StocksTrackerException(String message, Exception exception)
            : base(message, exception)
        {
        }

        /// <see cref="System.Exception"></see>
        public StocksTrackerException()
        {
        }
    }
}
