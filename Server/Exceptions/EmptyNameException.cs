using System;

namespace Server.Exceptions
{
    public class EmptyNameException : Exception
    {
        public EmptyNameException() : base("The name cannot be empty.")
        {
        }
        public EmptyNameException(string message) : base(message)
        {
        }

        public EmptyNameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
