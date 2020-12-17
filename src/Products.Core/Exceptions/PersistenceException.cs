using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Core.Exceptions
{
    public class PersistenceException : Exception
    {
        public PersistenceException()
        {
        }

        public PersistenceException(string message)
            : base(message)
        {
        }

        public PersistenceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
