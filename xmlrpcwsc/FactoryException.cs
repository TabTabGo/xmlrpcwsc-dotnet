using System;

namespace XmlRpc {

    /// <summary>
    /// Factory exception
    /// </summary>
    public class FactoryException : Exception {

        public FactoryException() {
        }

        public FactoryException(string message) : base(message) {
        }

        public FactoryException(string message, Exception inner) : base(message, inner) {
        }
    }
}

