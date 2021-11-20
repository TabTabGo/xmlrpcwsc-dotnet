using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace XmlRpc {

    /// <summary>
    /// XML-RPC response
    /// </summary>
    public class XmlRpcResponse {

        private bool isFault;
        private int faultCode;
        private string faultString;
        private object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRpc.XmlRpcResponse"/> class
        /// </summary>
        /// <param name="faultCode">Fault code</param>
        /// <param name="faultString">Fault string</param>
        public XmlRpcResponse(int faultCode, string faultString) {
            this.isFault = true;
            this.faultCode = faultCode;
            this.faultString = faultString;
            this.value = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRpc.XmlRpcResponse"/> class
        /// </summary>
        /// <param name="faultCode">Fault code</param>
        /// <param name="faultString">Fault string</param>
        /// <param name="value">Value.</param>
        public XmlRpcResponse(int faultCode, string faultString, object value) {
            this.isFault = true;
            this.faultCode = faultCode;
            this.faultString = faultString;
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRpc.XmlRpcResponse"/> class
        /// </summary>
        /// <param name="value">Value</param>
        public XmlRpcResponse(object value) {
            this.isFault = false;
            this.faultCode = -1;
            this.faultString = null;
            this.value = value;
        }

        /// <summary>
        /// Determines whether this instance is fault
        /// </summary>
        /// <returns><c>true</c> if this instance is fault; otherwise, <c>false</c></returns>
        public bool IsFault() {
            return isFault;
        }

        /// <summary>
        /// Gets the fault code
        /// </summary>
        /// <returns>The fault code</returns>
        public int GetFaultCode() {
            return faultCode;
        }

        /// <summary>
        /// Gets the fault string
        /// </summary>
        /// <returns>The fault string</returns>
        public string GetFaultString() {
            return faultString;
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        /// <returns>The value</returns>
        public object GetObject() {
            return value;
        }

        /// <summary>
        /// Determines whether this instance is null
        /// </summary>
        /// <returns><c>true</c> if this instance is null; otherwise, <c>false</c></returns>
        public bool IsNull() {
            return GetObject() == null;
        }

        /// <summary>
        /// Determines whether this instance is int
        /// </summary>
        /// <returns><c>true</c> if this instance is int; otherwise, <c>false</c></returns>
        public bool IsInt() {
            return GetObject() is int;
        }

        /// <summary>
        /// Gets the value int
        /// </summary>
        /// <returns>The value int</returns>
        public int GetInt() {
            if (IsNull())
                throw new NullReferenceException("The value is null");
            else if (IsInt())
                return (int)GetObject();
            else
                throw new InvalidCastException("The value is not of type int");
        }

        /// <summary>
        /// Determines whether this instance is boolean
        /// </summary>
        /// <returns><c>true</c> if this instance is boolean; otherwise, <c>false</c></returns>
        public bool IsBoolean() {
            return GetObject() is bool;
        }

        /// <summary>
        /// Gets the value boolean
        /// </summary>
        /// <returns><c>true</c>, if value boolean was gotten, <c>false</c> otherwise</returns>
        public bool GetBoolean() {
            if (IsNull())
                throw new NullReferenceException("The value is null");
            else if (IsBoolean())
                return (bool)GetObject();
            else
                throw new InvalidCastException("The value is not of type bool");
        }

        /// <summary>
        /// Determines whether this instance is string
        /// </summary>
        /// <returns><c>true</c> if this instance is string; otherwise, <c>false</c></returns>
        public bool IsString() {
            return GetObject() is string;
        }

        /// <summary>
        /// Gets the value string
        /// </summary>
        /// <returns>The value string</returns>
        public string GetString() {
            if (IsNull())
                throw new NullReferenceException("The value is null");
            else
                return ObjectToString(GetObject());
        }

        /// <summary>
        /// Objects to string
        /// </summary>
        /// <returns>The to string</returns>
        /// <param name="value">Value</param>
        private string ObjectToString(object value) {
            if (value is List<object>) {
                
                StringBuilder stringReturn = new StringBuilder();
                stringReturn.Append("[");
                foreach (object temp in (List<object>) value) {
                    stringReturn.Append(ObjectToString(temp));
                    stringReturn.Append(", ");
                }
                if (((List<object>)value).Count > 0)
                    stringReturn.Remove(stringReturn.Length - 2, 2);
                stringReturn.Append("]");
                return stringReturn.ToString();

            } else if (value is Dictionary<string, object>) {
                StringBuilder stringReturn = new StringBuilder();
                stringReturn.Append("{");
                foreach (KeyValuePair<string,object> temp in (Dictionary<string, object>) value) {                    
                    stringReturn.Append(temp.Key);
                    stringReturn.Append(": ");
                    stringReturn.Append(ObjectToString(temp.Value));
                    stringReturn.Append(", ");
                }
                if (((Dictionary<string, object>)value).Count > 0)
                    stringReturn.Remove(stringReturn.Length - 2, 2);
                stringReturn.Append("}");
                return stringReturn.ToString();
            } else if (value is byte[]) {
                return Convert.ToBase64String((byte[])value);
            } else {
                return value.ToString();
            }

        }

        /// <summary>
        /// Determines whether this instance is double
        /// </summary>
        /// <returns><c>true</c> if this instance is double; otherwise, <c>false</c></returns>
        public bool IsDouble() {
            return GetObject() is double;
        }

        /// <summary>
        /// Gets the value double
        /// </summary>
        /// <returns>The value double</returns>
        public double GetDouble() {
            if (IsNull())
                throw new NullReferenceException("The value is null");
            else if (IsDouble())
                return (double)GetObject();
            else
                throw new InvalidCastException("The value is not of type double");
        }

        /// <summary>
        /// Determines whether this instance is date time
        /// </summary>
        /// <returns><c>true</c> if this instance is date time; otherwise, <c>false</c></returns>
        public bool IsDateTime() {
            return GetObject() is DateTime;
        }

        /// <summary>
        /// Gets the value date time
        /// </summary>
        /// <returns>The value date time</returns>
        public DateTime GetDateTime() {
            if (IsNull())
                throw new NullReferenceException("The value is null");
            else if (IsDateTime())
                return (DateTime)GetObject();
            else
                throw new InvalidCastException("The value is not of type DateTime");
        }

        /// <summary>
        /// Determines whether this instance is byte
        /// </summary>
        /// <returns><c>true</c> if this instance is byte; otherwise, <c>false</c></returns>
        public bool IsByte() {
            return GetObject() is byte[];
        }

        /// <summary>
        /// Gets the value byte
        /// </summary>
        /// <returns>The value byte</returns>
        public byte[] GetByte() {
            if (IsNull())
                throw new NullReferenceException("The value is null");
            else if (IsByte())
                return (byte[])GetObject();
            else
                throw new InvalidCastException("The value is not of type byte[]");
        }

        /// <summary>
        /// Determines whether this instance is array
        /// </summary>
        /// <returns><c>true</c> if this instance is array; otherwise, <c>false</c></returns>
        public bool IsArray() {
            return GetObject() is List<object>;
        }

        /// <summary>
        /// Gets the value array
        /// </summary>
        /// <returns>The value array</returns>
        public List<object> GetArray() {
            if (IsNull())
                throw new NullReferenceException("The value is null");
            else if (IsArray())
                return (List<object>)GetObject();
            else
                throw new InvalidCastException("The value is not of type List<object>");
        }

        /// <summary>
        /// Determines whether this instance is struct
        /// </summary>
        /// <returns><c>true</c> if this instance is struct; otherwise, <c>false</c></returns>
        public bool IsStruct() {
            return GetObject() is Dictionary<string, object>;
        }

        /// <summary>
        /// Gets the value struct
        /// </summary>
        /// <returns>The value struct</returns>
        public Dictionary<string, object> GetStruct() {
            if (IsNull())
                throw new NullReferenceException("The value is null");
            else if (IsStruct())
                return (Dictionary<string, object>)GetObject();
            else
                throw new InvalidCastException("The value is not of type Dictionary<string, object>");
        }

    }
}

