using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace XmlRpc {

    /// <summary>
    /// XML-RPC client
    /// </summary>
    public class XmlRpcClient : WebServiceConnection {

        protected XmlDocument xmlRequest;
        protected XmlDocument xmlResponse;
        protected XmlRpcRequest request;
        protected XmlRpcResponse response;

        /// <summary>
        /// Gets the xml request
        /// </summary>
        /// <returns>The xml request</returns>
        public XmlDocument GetXmlRequest() {
            return xmlRequest;
        }

        /// <summary>
        /// Gets the xml response
        /// </summary>
        /// <returns>The xml response</returns>
        public XmlDocument GetXmlResponse() {
            return xmlResponse;
        }

        /// <summary>
        /// Writes the request
        /// </summary>
        /// <param name="fileName">File name</param>
        public void WriteRequest(String fileName) {
            TextWriter streamWriter = new StreamWriter(fileName, false, Encoding.UTF8);
            WriteRequest(streamWriter);
        }

        /// <summary>
        /// Writes the request
        /// </summary>
        /// <param name="outStream">Out stream</param>
        public void WriteRequest(TextWriter outStream) {
            xmlRequest.Save(outStream);
        }

        /// <summary>
        /// Writes the response
        /// </summary>
        /// <param name="fileName">File name</param>
        public void WriteResponse(String fileName) {
            TextWriter streamWriter = new StreamWriter(fileName, false, Encoding.UTF8);
            WriteResponse(streamWriter);
        }

        /// <summary>
        /// Writes the response
        /// </summary>
        /// <param name="outStream">Out stream</param>
        public void WriteResponse(TextWriter outStream) {
            xmlResponse.Save(outStream);           
        }

        /// <summary>
        /// Execute the specified methodName and parameters
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="parameters">Parameters</param>
        public XmlRpcResponse Execute(string methodName, List<object> parameters){
            return Execute(new XmlRpcRequest(methodName, parameters));
        }

        /// <summary>
        /// Execute the specified methodName and parameters
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="parameters">Parameters</param>
        public XmlRpcResponse Execute(string methodName, object[] parameters){
            return Execute(new XmlRpcRequest(methodName, parameters));
        }

        /// <summary>
        /// Execute the specified methodName
        /// </summary>
        /// <param name="methodName">Method name</param>
        public XmlRpcResponse Execute(string methodName){
            return Execute(new XmlRpcRequest(methodName));
        }

        /// <summary>
        /// Execute the specified request
        /// </summary>
        /// <param name="request">Request</param>
        public virtual XmlRpcResponse Execute(XmlRpcRequest request){
            this.request = request;

            XmlDocument xmlRequest = RequestFactory.BuildRequest(request);
            this.xmlRequest = xmlRequest;

            XmlDocument xmlResponse = SendRequest(xmlRequest);
            this.xmlResponse = xmlResponse;

            this.response = ResponseFactory.BuildResponse(xmlResponse);

            return response;
        }
    }
}

