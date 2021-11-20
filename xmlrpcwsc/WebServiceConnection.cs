using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Threading;

namespace XmlRpc {

    /// <summary>
    /// This class send a stream data xml.
    /// </summary>
    public class WebServiceConnection {

        public static readonly string DefaultContentType = "text/xml; charset=UTF-8";
        public static readonly string DefaultRequestMethod = "POST";
        public static readonly int DefaultTimeout = 5000;
        public static readonly int DefaultAttempts = 1;
        public static readonly int DefaultAttemptsTimeout = 500;
        public static readonly int DefaultConnectionLimit = 2;
        public static readonly ICredentials DefaultCredentials = CredentialCache.DefaultCredentials;
        public static readonly IWebProxy DefaultWebProxy = WebRequest.DefaultWebProxy;

        private int connectionLimit;
        private int attempts;
        private int attemptsTimeout;
        private int timeout;
        private string appName;

        /// <summary>
        /// For connection simultaneously
        /// </summary>
        public int ConnectionLimit {
            get {
                return connectionLimit;
            }
            set {
                if (value <= 0)
                    connectionLimit = DefaultConnectionLimit;
                else
                    connectionLimit = value;
            }
        }

        /// <summary>
        /// Attempts for request
        /// </summary>
        public int Attempts {
            get {
                return attempts;
            }
            set {
                if (value <= 0)
                    attempts = DefaultAttempts;
                else
                    attempts = value;
            }
        }

        /// <summary>
        /// Gets or sets the attempts timeout. Default: WebServiceConnection.DefaultAttemptsTimeout
        /// </summary>
        /// <value>The attempts timeout</value>
        public int AttemptsTimeout {
            get {
                return attemptsTimeout;
            }
            set {
                if (value <= 0)
                    attemptsTimeout = DefaultTimeout;
                else
                    attemptsTimeout = value;
            }
        }

        /// <summary>
        /// Timeout for request. Default: WebServiceConnection.DefaultTimeout
        /// </summary>
        public int Timeout {
            get {
                return timeout;
            }
            set {
                if (value <= 0)
                    timeout = DefaultTimeout;
                else
                    timeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the app
        /// </summary>
        /// <value>The name of the app</value>
        public string AppName {       
            get {
                return appName;
            }
            set {
                if (appName == null)
                    appName = "";
                else
                    appName = value;
            }

        }

        /// <summary>
        /// total Attempts for request
        /// </summary>
        public int AttemptsRequest {
            get;
            protected set;
        }

        /// <summary>
        /// Total time for request
        /// </summary>
        public int TimeRequest {
            get;
            protected set;
        }

        /// <summary>
        /// Proxy. Default: WebServiceConnection.DefaultProxy
        /// </summary>
        public IWebProxy Proxy {
            get;
            set;
        }

        /// <summary>
        /// Credentials. Default: WebServiceConnection.DefaultCredentials
        /// </summary>
        public ICredentials Credentials {
            get;
            set;
        }

        /// <summary>
        /// Url for connect
        /// </summary>
        public string Url {
            get;
            set;
        }

        /// <summary>
        /// Path for connect
        /// </summary>
        public string Path {
            get;
            set;
        }

        /// <summary>
        /// Data typ. Default: WebServiceConnection.DefaultContentType
        /// </summary>
        public string ContentType {
            get;
            set;
        }

        /// <summary>
        /// Method for request. Default: WebServiceConnection.DefaultRequestMethod
        /// </summary>
        public string RequestMethod {
            get;
            set;
        }

        /// <summary>
        /// Gets the user agent
        /// </summary>
        /// <returns>The user agent</returns>
        public string GetUserAgent() {
            return string.Format("{0} ({1}/{2}/{3}/{4}) {5}", ComponentInfo.Name, ComponentInfo.ComponentName, ComponentInfo.Version, ".NET", Environment.OSVersion, AppName).Trim();
        }

        /// <summary>
        /// Build the Url for WebService Connection
        /// </summary>
        /// <returns>Url for create HttpWebRequest</returns>
        private string GetWebServiceUrl() {
            if (Path == null)
                return Url;

            string urlTemp = Url;
            if (urlTemp.EndsWith("/"))
                urlTemp = urlTemp.Substring(0, urlTemp.Length - 1);

            string path = Path;
            if (path.StartsWith("/"))
                path = path.Substring(1);

            return String.Format("{0}/{1}", urlTemp, path);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebServiceConnection() {
            Timeout = DefaultTimeout;
            ContentType = DefaultContentType;
            RequestMethod = DefaultRequestMethod;
            Credentials = DefaultCredentials;
            Attempts = DefaultAttempts;
            ConnectionLimit = DefaultConnectionLimit;
            AttemptsTimeout = DefaultAttemptsTimeout;
            Proxy = DefaultWebProxy;
            Url = "";
            AppName = "";
        }

        /// <summary>
        /// FOR SSL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool OnValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            return true;
        }

        /// <summary>
        /// Send data in xml format
        /// </summary>
        /// <param name="dataRequest">Xml Document</param>
        /// <returns>Xml Response</returns>
        public XmlDocument SendRequest(XmlDocument dataRequest) {
            return SendRequest(dataRequest.OuterXml);
        }

        /// <summary>
        /// Send string data
        /// </summary>
        /// <param name="dataRequest">Data</param>
        /// <returns>Xml Response</returns>
        public XmlDocument SendRequest(string dataRequest) {
            
            if (string.IsNullOrEmpty(Url))
                throw new WebServiceException("URL must be different than empty or null");

            ServicePointManager.ServerCertificateValidationCallback = OnValidateCertificate;
            ServicePointManager.DefaultConnectionLimit = ConnectionLimit;
            ServicePointManager.Expect100Continue = false;
            
            TimeSpan startTime = TimeSpan.FromMilliseconds(Environment.TickCount);
            AttemptsRequest = 0;
            bool successful = false;
            string dataResponse = "";   
            XmlDocument xmlDocument = new XmlDocument();

            while (!successful) {
                AttemptsRequest++;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetWebServiceUrl());
                request.Timeout = Timeout;
                request.KeepAlive = false;
                request.Method = RequestMethod;
                request.UserAgent = GetUserAgent();

                byte[] bytesData = Encoding.UTF8.GetBytes(dataRequest);
                request.ContentType = ContentType;
                request.ContentLength = bytesData.Length;

                if (Proxy != null)
                    request.Proxy = Proxy;

                if (Credentials != null)
                    request.Credentials = Credentials;
            
                try {
                    Stream requestStreamData = request.GetRequestStream();
                    requestStreamData.Write(bytesData, 0, bytesData.Length);
                    requestStreamData.Close();

                    WebResponse response = request.GetResponse();

                    Stream responseStreamData = response.GetResponseStream();
                    StreamReader readResponseStreamData = new StreamReader(responseStreamData, Encoding.UTF8);
                    dataResponse = readResponseStreamData.ReadToEnd();

                    readResponseStreamData.Close();
                    responseStreamData.Close();
                    response.Close();

                    xmlDocument.LoadXml(dataResponse);

                    successful = true;
                } catch (Exception e) {                    
                    if (AttemptsRequest >= Attempts) {
                        TimeRequest = (int)TimeSpan.FromMilliseconds(Environment.TickCount).Subtract(startTime).Duration().TotalMilliseconds;
                        if (e.GetType().Equals(typeof(WebException))) {
                            WebException we = (WebException)e;
                            if (we.Status == WebExceptionStatus.Timeout) {
                                throw new WebServiceTimeoutException("Timeout exception, operation has expired", e);
                            }
                        }                        
                        throw new WebServiceException("Error sending request", e);
                    } else {
                        Thread.Sleep(AttemptsTimeout);
                        continue;
                    }
                }
            }

            TimeRequest = (int)TimeSpan.FromMilliseconds(Environment.TickCount).Subtract(startTime).Duration().TotalMilliseconds;

            return xmlDocument;
        }

    }
}
