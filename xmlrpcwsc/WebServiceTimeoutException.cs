using System;
using System.Collections.Generic;
using System.Text;

namespace XmlRpc {

    /// <summary>
    /// Web service timeout exception
    /// </summary>
    public class WebServiceTimeoutException : WebServiceException {

        public WebServiceTimeoutException() {
        }

        public WebServiceTimeoutException(string message) : base(message) {
        }

        public WebServiceTimeoutException(string message, Exception inner) : base(message, inner) {
        }
    }
}
