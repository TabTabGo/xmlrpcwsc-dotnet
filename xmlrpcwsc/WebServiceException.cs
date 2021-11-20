using System;

namespace XmlRpc {

    /// <summary>
    /// Web service exception
    /// </summary>
    public class WebServiceException : Exception {
        
        public WebServiceException() {
        }

        public WebServiceException(string message) : base(message) {
        }

        public WebServiceException(string message, Exception inner) : base(message, inner) {
        }
    }
}

