using System;
using System.Collections.Generic;

namespace XmlRpc {

    /// <summary>
    /// XML-RPC request
    /// </summary>
    public class XmlRpcRequest {
        
        /// <summary>
        /// Gets or sets the name of the method
        /// </summary>
        /// <value>The name of the method</value>
        public string MethodName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parameters
        /// </summary>
        /// <value>The parameters</value>
        private List<object> Params {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRpc.XmlRcpRequest"/> class
        /// </summary>
        public XmlRpcRequest() {
            this.MethodName = "";
            this.Params = new List<object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRpc.XmlRcpRequest"/> class
        /// </summary>
        /// <param name="methodName">Method name</param>
        public XmlRpcRequest(string methodName) {
            this.MethodName = methodName ?? "";
            this.Params = new List<object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRpc.XmlRpcRequest"/> class
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="parameters">Parameters</param>
        public XmlRpcRequest(string methodName, params object[] parameters) {
            this.MethodName = methodName ?? "";
            this.Params = new List<object>();
            if (parameters != null)
                this.Params.AddRange(parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRpc.XmlRcpRequest"/> class
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="parameters">Parameters.</param>
        public XmlRpcRequest(string methodName, List<object> parameters) {
            this.MethodName = methodName ?? "";
            this.Params = parameters ?? new List<object>();
        }

        /// <summary>
        /// Adds the parameter, if param is DateTime uses DateTime.UtcNow, for dateTime.iso8601 conversion
        /// </summary>
        /// <param name="param">Parameter</param>
        public XmlRpcRequest AddParam(object param) {
            Params.Add(param);
            return this;
        }

        /// <summary>
        /// Adds the parameters
        /// </summary>
        /// <param name="parameters">Parameters</param>
        public XmlRpcRequest AddParams(params object[] list) {
            Params.AddRange(list);
            return this;
        }

        /// <summary>
        /// Adds the parameter array
        /// </summary>
        /// <param name="list">List</param>
        public XmlRpcRequest AddParamArray(params object[] list) {
            AddParam(XmlRpcParameter.AsArray(list));
            return this;
        }

        /// <summary>
        /// Adds the parameter struct
        /// </summary>
        /// <param name="list">List</param>
        public XmlRpcRequest AddParamStruct(params KeyValuePair<string,object>[] list) {
            AddParam(XmlRpcParameter.AsStruct(list));
            return this;
        }

        /// <summary>
        /// Removes the parameter
        /// </summary>
        /// <returns>The parameter</returns>
        /// <param name="param">Parameter</param>
        public void RemoveParam(object param) {
            Params.Remove(param);
        }

        /// <summary>
        /// Removes the parameter
        /// </summary>
        /// <returns>The parameter</returns>
        /// <param name="pos">Position</param>
        public object RemoveParam(int pos) {
            object operation = Params[pos];
            RemoveParam(operation);
            return operation;
        }

        /// <summary>
        /// Gets the parameter
        /// </summary>
        /// <returns>The parameter</returns>
        /// <param name="pos">Position</param>
        public object GetParam(int pos) {
            return Params[pos];
        }

        /// <summary>
        /// Gets the parameters
        /// </summary>
        /// <returns>The parameters</returns>
        public List<object> GetParams() {
            List<object> temp = new List<object>();
            temp.AddRange(Params);
            return temp;
        }

        /// <summary>
        /// Gets the parameters count
        /// </summary>
        /// <returns>The parameters count</returns>
        public int GetParamsCount() {
            return Params.Count;
        }

        /// <summary>
        /// Clear this instance
        /// </summary>
        public void Clear() {
            Params.Clear();
        }

    }
}

