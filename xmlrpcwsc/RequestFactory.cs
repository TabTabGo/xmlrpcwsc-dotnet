using System;
using System.Xml;
using System.Collections.Generic;

namespace XmlRpc {

    /// <summary>
    /// XML-RPC request factory
    /// </summary>
    public class RequestFactory {
              
        /// <summary>
        /// Builds the xml request
        /// </summary>
        /// <returns>The xml request</returns>
        /// <param name="request">Request</param>
        public static XmlDocument BuildRequest(XmlRpcRequest request) {
            XmlDocument doc = new XmlDocument();

            XmlElement xmlMethodCall = doc.CreateElement("methodCall");
            doc.AppendChild(xmlMethodCall);

            XmlElement xmlMethodName = doc.CreateElement("methodName");
            xmlMethodName.InnerText = request.MethodName ?? "";
            xmlMethodCall.AppendChild(xmlMethodName);

            XmlElement xmlParams = doc.CreateElement("params");
            xmlMethodCall.AppendChild(xmlParams);

            if (request.GetParamsCount() <= 0)
                return doc;

            List<object> parameters = request.GetParams();
            
            foreach (object parameter in parameters) {
                XmlElement xmlParam = doc.CreateElement("param");
                xmlParams.AppendChild(xmlParam);
                xmlParam.AppendChild(doc.ImportNode(BuildXmlValue(parameter), true));
            }

            return doc;
        }

        /// <summary>
        /// Builds the xml value
        /// </summary>
        /// <returns>The xml value</returns>
        /// <param name="value">Value</param>
        private static XmlElement BuildXmlValue(object value) {
            XmlDocument doc = new XmlDocument();
            XmlElement xmlValue = doc.CreateElement("value");

            if (value is bool) {
                
                XmlElement xmlType = doc.CreateElement("boolean");
                xmlType.InnerText = (bool)value ? "1" : "0";
                xmlValue.AppendChild(xmlType);

            } else if (value is int) {
                
                XmlElement xmlType = doc.CreateElement("int");
                xmlType.InnerText = value.ToString();
                xmlValue.AppendChild(xmlType);

            } else if (value is double) {
                
                XmlElement xmlType = doc.CreateElement("double");
                xmlType.InnerText = value.ToString();
                xmlValue.AppendChild(xmlType);

            } else if (value is DateTime) {
                
                XmlElement xmlType = doc.CreateElement("dateTime.iso8601");
                // EXAMPLE 19980717T14:08:55
                xmlType.InnerText = string.Format("{0:yyyyMMdd'T'HH:mm:ss}", value);
                xmlValue.AppendChild(xmlType);

            } else if (value is byte[]) {
                
                XmlElement xmlType = doc.CreateElement("base64");
                xmlType.InnerText = Convert.ToBase64String((byte[])value);
                xmlValue.AppendChild(xmlType);

            } else if (value is Dictionary<string, object>) {
                
                XmlElement xmlType = doc.CreateElement("struct");
                xmlValue.AppendChild(xmlType);

                foreach (KeyValuePair<string, object> item in (Dictionary<string, object>) value) {                    
                    XmlElement xmlMember = doc.CreateElement("member");
                    xmlType.AppendChild(xmlMember);

                    XmlElement xmlName = doc.CreateElement("name");
                    xmlName.InnerText = item.Key;
                    xmlMember.AppendChild(xmlName);
                    xmlMember.AppendChild(doc.ImportNode(BuildXmlValue(item.Value), true));
                }

            } else if (value is List<object>) {

                XmlElement xmlType = doc.CreateElement("array");
                xmlValue.AppendChild(xmlType);

                XmlElement xmlData = doc.CreateElement("data");
                xmlType.AppendChild(xmlData);   

                foreach (object item in (List<object>) value) {   
                    xmlData.AppendChild(doc.ImportNode(BuildXmlValue(item), true));
                }

            } else if (value is object[]) {

                XmlElement xmlType = doc.CreateElement("array");
                xmlValue.AppendChild(xmlType);

                XmlElement xmlData = doc.CreateElement("data");
                xmlType.AppendChild(xmlData);   

                foreach (object item in (object[]) value) {   
                    xmlData.AppendChild(doc.ImportNode(BuildXmlValue(item), true));
                }

            } else {
                
                XmlElement xmlType = doc.CreateElement("string");
                xmlType.InnerText = value.ToString();
                xmlValue.AppendChild(xmlType);

            }

            return xmlValue;
        }
    }
}

