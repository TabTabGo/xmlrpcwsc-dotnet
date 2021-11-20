using System;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;

namespace XmlRpc {
    
    /// <summary>
    /// XML-RPC response factory
    /// </summary>
    public class ResponseFactory {
        
        public static XmlRpcResponse BuildResponse(XmlDocument response) {            

            XmlNodeList xmlMethodResponseList = response.GetElementsByTagName("methodResponse");

            if (xmlMethodResponseList.Count <= 0) {
                throw new FactoryException("XML file malformed, no tag methodResponse");
            }      

            XmlElement xmlMethodResponse = (XmlElement)xmlMethodResponseList.Item(0);

            if (xmlMethodResponse.FirstChild == null) {
                throw new FactoryException("XML file malformed, methodResponse child");
            }

            XmlElement xmlFirst = (XmlElement)xmlMethodResponse.FirstChild;

            if (xmlFirst.Name.Equals("fault")) {
                object objectValue = BuildObjectValue((XmlElement)xmlFirst.FirstChild);                       
                Dictionary<string, object> faultStruct = (Dictionary<string, object>)objectValue;
                int faultCode = (int)faultStruct["faultCode"];
                string faultString = (string)faultStruct["faultString"];
                return new XmlRpcResponse(faultCode, faultString, faultStruct);
            } else if (xmlFirst.Name.Equals("params")) {
                XmlElement xmlParam = (XmlElement)xmlFirst.FirstChild;
                if (xmlParam == null || !xmlParam.Name.Equals("param")) {                    
                    throw new FactoryException("XML file malformed, no tag param");
                }

                return new XmlRpcResponse(BuildObjectValue((XmlElement)xmlParam.FirstChild)); 
            } else {
                throw new FactoryException("XML file malformed, methodResponse child");
            }
        }

        private static object BuildObjectValue(XmlElement value) {
            
            if (value == null || !value.Name.Equals("value")) {
                throw new FactoryException("XML file malformed, need value tag");
            }

            XmlElement xmlType = (XmlElement)value.FirstChild;

            if (xmlType == null) {
                throw new FactoryException("XML file malformed, need type tag in value tag");
            }

            string stringType = xmlType.Name;

            if (stringType.Equals("int") || stringType.Equals("i4")) { 
                
                return int.Parse(xmlType.InnerText);

            } else if (stringType.Equals("boolean")) {
                
                return xmlType.InnerText.Equals("1");

            } else if (stringType.Equals("double")) {
                
                return double.Parse(xmlType.InnerText);

            } else if (stringType.Equals("base64")) {
                
                return Convert.FromBase64String(xmlType.InnerText);

            } else if (stringType.Equals("dateTime.iso8601") || stringType.Equals("dateTime") || stringType.Equals("date")) {

                // EXAMPLE 19980717T14:08:55
                return DateTime.ParseExact(xmlType.InnerText, "yyyyMMdd'T'HH:mm:ss", CultureInfo.InvariantCulture);

            } else if (stringType.Equals("array")) {
                
                List<object> objectValue = new List<object>();
                XmlElement xmlData = (XmlElement)xmlType.FirstChild;

                if (xmlData == null || !xmlData.Name.Equals("data")) {
                    throw new FactoryException("XML file malformed, need data tag in array tag");
                }

                IEnumerator iteValues = xmlData.GetEnumerator();
                while (iteValues.MoveNext()) {
                    XmlElement xmlValue = (XmlElement)iteValues.Current;
                    objectValue.Add(BuildObjectValue(xmlValue));
                }
                return objectValue;

            } else if (stringType.Equals("struct")) {
                
                Dictionary<string, object> objectValue = new Dictionary<string, object>();               
                IEnumerator iteMember = xmlType.GetEnumerator();
                while (iteMember.MoveNext()) {
                    XmlElement xmlMember = (XmlElement)iteMember.Current;

                    IEnumerator iteMemberValues = xmlMember.GetEnumerator();

                    string name = "";
                    object innerObject = null;

                    while (iteMemberValues.MoveNext()) {
                        XmlElement xmlMemberValue = (XmlElement)iteMemberValues.Current;
                        if (xmlMemberValue.Name == "name") {
                            name = xmlMemberValue.InnerText;
                        } else if (xmlMemberValue.Name == "value") {
                            innerObject = BuildObjectValue(xmlMemberValue);
                        }
                    }
                    if (innerObject != null && !string.IsNullOrEmpty(name))
                        objectValue.Add(name, innerObject);
                }
                return objectValue;

            } else {
                return xmlType.InnerText;
            }

        }
    }
}

