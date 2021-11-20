using System;
using System.Collections.Generic;

namespace XmlRpc {

    /// <summary>
    /// XML-RPC Parameter helper
    /// </summary>
    public class XmlRpcParameter {

         /// <summary>
        /// Empty the struct.
        /// </summary>
        /// <returns>The struct</returns>
        public static Dictionary<string,object> EmptyStruct() {
            return new Dictionary<string,object>();
        }

        /// <summary>
        /// Empty array
        /// </summary>
        /// <returns>The array</returns>
        public static List<object> EmptyArray() {
            return new List<object>();
        }

        /// <summary>
        /// As array
        /// </summary>
        /// <returns>The array.</returns>
        /// <param name="list">List.</param>
        public static List<object> AsArray(params object[] list) {
            List<object> listReturn = new List<object>();
            listReturn.AddRange(list);
            return listReturn;
        }

        /// <summary>
        /// As struct
        /// </summary>
        /// <returns>The struct</returns>
        /// <param name="list">List</param>
        public static Dictionary<string, object> AsStruct(params KeyValuePair<string,object>[] list) {
            Dictionary<string,object> dictReturn = new Dictionary<string,object>();

            foreach (KeyValuePair<string,object> pair in list) {
                dictReturn.Add(pair.Key, pair.Value);
            }

            return dictReturn;
        }

        /// <summary>
        /// As member
        /// </summary>
        /// <returns>The member</returns>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static KeyValuePair<string,object> AsMember(string key, object value) {
            KeyValuePair<string,object> pairReturn = new KeyValuePair<string, object>(key, value);
            return pairReturn;
        }
    }
}
