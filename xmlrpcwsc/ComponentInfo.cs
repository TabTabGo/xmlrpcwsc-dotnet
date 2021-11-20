using System;
using System.Collections.Generic;

namespace XmlRpc {

    /// <summary>
    /// Component info
    /// </summary>
    public sealed class ComponentInfo {
        
        public static readonly string Name = "XML-RPC Web Service Client";
        public static readonly string ComponentName = "xmlrpcwsc";
        public static readonly string Version = "1.3.0";

        /// <summary>
        /// To the dictionary
        /// </summary>
        /// <returns>The dictionary</returns>
        public static Dictionary<string, string> ToDictionary() {
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("Name", Name);
            info.Add("ComponentName", ComponentName);
            info.Add("Version", Version);
            return info;
        }
    }
}

