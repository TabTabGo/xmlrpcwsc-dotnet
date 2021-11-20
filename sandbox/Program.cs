using System;
using XmlRpc;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace sandbox {

    /// <summary>
    /// This class, test the xmlrpc component on odoo 9
    /// </summary>
    class MainClass {

        public static string Url = "http://localhost:8069/xmlrpc/2", db = "odoo9", pass = "admin", user = "admin";

        public static void TestRequestXml() {
            XmlRpcRequest request = new XmlRpcRequest("version");
            request.AddParam(false);
            request.AddParam(3);
            request.AddParam(4.9);
            request.AddParam(DateTime.Now);
            request.AddParam(DateTime.UtcNow);
            request.AddParam(Encoding.UTF8.GetBytes("hello"));

            Dictionary<string, object> dictest = new Dictionary<string, object>();
            dictest.Add("hello", "hello");
            // request.AddParam(dictest);

            List<object> listtest = new List<object>();
            listtest.Add(3);
            listtest.Add("hello");
            listtest.Add(dictest);
            request.AddParam(listtest);

            XmlDocument xmlRequest = RequestFactory.BuildRequest(request);

            xmlRequest.Save(Console.Out);

            XmlRpcClient client = new XmlRpcClient();
            client.AppName = "Test";
            Console.WriteLine("\n");
            Console.WriteLine(client.GetUserAgent());
        }

        public static void TestReadVersion() {
            XmlRpcClient client = new XmlRpcClient();
            client.Url = Url;
            client.Path = "common";

            XmlRpcResponse response = client.Execute("version");

            Console.WriteLine("version");
            Console.WriteLine("REQUEST: ");
            client.WriteRequest(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("RESPONSE: ");
            client.WriteResponse(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            if (response.IsFault()) {
                Console.WriteLine(response.GetFaultString());
            } else {
                Console.WriteLine(response.GetString());
            }
        }

        public static void TestReadRecords() {
            XmlRpcClient client = new XmlRpcClient();
            client.Url = Url;
            client.Path = "common";           

            // LOGIN

            XmlRpcRequest requestLogin = new XmlRpcRequest("authenticate");
            requestLogin.AddParams(db, user, pass, XmlRpcParameter.EmptyStruct());

            XmlRpcResponse responseLogin = client.Execute(requestLogin);

            // Console.WriteLine("authenticate");
            // Console.WriteLine("REQUEST: ");
            // client.WriteRequest(Console.Out);

            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("RESPONSE: ");
            // client.WriteResponse(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("LOGIN: ");
            if (responseLogin.IsFault()) {
                Console.WriteLine(responseLogin.GetFaultString());
            } else {
                Console.WriteLine(responseLogin.GetString());
            }

            // SEARCH

            client.Path = "object";

            XmlRpcRequest requestSearch = new XmlRpcRequest("execute_kw");
            requestSearch.AddParams(db, responseLogin.GetInt(), pass, "res.partner", "search", 
                XmlRpcParameter.AsArray(
                    XmlRpcParameter.AsArray(
                        XmlRpcParameter.AsArray("is_company", "=", true), XmlRpcParameter.AsArray("customer", "=", true)
                    )
                )
            );

            requestSearch.AddParamStruct(
                XmlRpcParameter.AsMember("limit", 2)
            );

            XmlRpcResponse responseSearch = client.Execute(requestSearch);

            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("search");
            // Console.WriteLine("REQUEST: ");
            // client.WriteRequest(Console.Out);

            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("RESPONSE: ");
            // client.WriteResponse(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("SEARCH: ");
            if (responseSearch.IsFault()) {
                Console.WriteLine(responseSearch.GetFaultString());
            } else {
                Console.WriteLine(responseSearch.GetString());
            }

            // READ

            XmlRpcRequest requestRead = new XmlRpcRequest("execute_kw");
            requestRead.AddParams(db, responseLogin.GetInt(), pass, "res.partner", "read",                                           
                XmlRpcParameter.AsArray(
                    responseSearch.GetArray()
                )
            );

            requestRead.AddParamStruct(XmlRpcParameter.AsMember("fields", 
                    XmlRpcParameter.AsArray("name")
                )
            );

            XmlRpcResponse responseRead = client.Execute(requestRead);

            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("read");
            // Console.WriteLine("REQUEST: ");
            // client.WriteRequest(Console.Out);

            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("RESPONSE: ");
            // client.WriteResponse(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("READ: ");
            if (responseRead.IsFault()) {
                Console.WriteLine(responseRead.GetFaultString());
            } else {
                Console.WriteLine(responseRead.GetString());
            }
        }

        public static void TestResponseXml() {
            XmlDocument testDoc = new XmlDocument();
            // testDoc.AppendChild(testDoc.CreateElement("methodResponse"));
            // testDoc.LoadXml("<methodResponse><fault><value><struct><member><name>faultCode</name><value><int>1</int></value></member><member><name>faultString</name><value><string>Error</string></value></member></struct></value></fault></methodResponse>");
            testDoc.LoadXml("<methodResponse><params><param><value><array><data><value><int>7</int></value><value><int>11</int></value><value><int>8</int></value><value><int>44</int></value><value><int>10</int></value><value><int>12</int></value></data></array></value></param></params></methodResponse>");

            testDoc.Save(Console.Out);
            XmlRpcResponse response = ResponseFactory.BuildResponse(testDoc);

            if (response.IsFault()) {
                Console.WriteLine(response.GetFaultString());
            } else {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(response.GetString());
            }
        }     

        public static void TestSearchReadRecords() {
            XmlRpcClient client = new XmlRpcClient();
            client.Url = Url;
            client.Path = "common";           

            // LOGIN

            XmlRpcRequest requestLogin = new XmlRpcRequest("authenticate");
            requestLogin.AddParams(db, user, pass, XmlRpcParameter.EmptyStruct());

            XmlRpcResponse responseLogin = client.Execute(requestLogin);

            // Console.WriteLine("authenticate");
            // Console.WriteLine("REQUEST: ");
            // client.WriteRequest(Console.Out);

            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("RESPONSE: ");
            // client.WriteResponse(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("LOGIN: ");
            if (responseLogin.IsFault()) {
                Console.WriteLine(responseLogin.GetFaultString());
            } else {
                Console.WriteLine(responseLogin.GetString());
            }

            // SEARCH

            client.Path = "object";

            XmlRpcRequest requestSearch = new XmlRpcRequest("execute_kw");
            requestSearch.AddParams(db, responseLogin.GetInt(), pass, "res.partner", "search_read", 
                XmlRpcParameter.AsArray(
                    XmlRpcParameter.AsArray(
                        // XmlRpcParameter.AsArray("is_company", "=", true), XmlRpcRequest.AsArray("customer", "=", true)
                        XmlRpcParameter.AsArray("name", "ilike", "t")
                    )
                ),
                XmlRpcParameter.AsStruct(
                    XmlRpcParameter.AsMember("fields", XmlRpcParameter.AsArray("name", "email"))
                    // ,XmlRpcParameter.AsMember("limit", 2)
                )
            );                      

            XmlRpcResponse responseSearch = client.Execute(requestSearch);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("search");
            Console.WriteLine("REQUEST: ");
            client.WriteRequest(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("RESPONSE: ");
            

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("SEARCH: ");
            if (responseSearch.IsFault()) {
                Console.WriteLine(responseSearch.GetFaultString());
            } else {
                Console.WriteLine(responseSearch.GetString());
            }
           
        }

        public static void TestCreateRecord() {
            XmlRpcClient client = new XmlRpcClient();
            client.Url = Url;
            client.Path = "common";           

            // LOGIN

            XmlRpcRequest requestLogin = new XmlRpcRequest("authenticate");
            requestLogin.AddParams(db, user, pass, XmlRpcParameter.EmptyStruct());

            XmlRpcResponse responseLogin = client.Execute(requestLogin);

            Console.WriteLine("authenticate");
            Console.WriteLine("REQUEST: ");
            client.WriteRequest(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("RESPONSE: ");
            client.WriteResponse(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("LOGIN: ");
            if (responseLogin.IsFault()) {
                Console.WriteLine(responseLogin.GetFaultString());
            } else {
                Console.WriteLine(responseLogin.GetString());
            }

            // CREATE

            client.Path = "object"; 

            XmlRpcRequest requestCreate = new XmlRpcRequest("execute_kw");
            requestCreate.AddParams(db, responseLogin.GetInt(), pass, "res.partner", "create",                                           
                XmlRpcParameter.AsArray(
                    XmlRpcParameter.AsStruct(
                        XmlRpcParameter.AsMember("name", "Albert Einstein")
                        , XmlRpcParameter.AsMember("image", Convert.ToBase64String(File.ReadAllBytes("img/einstein.jpg")))
                        , XmlRpcParameter.AsMember("email", "albert.einstein@email.com")
                    )
                )
            );

            XmlRpcResponse responseCreate = client.Execute(requestCreate);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("create");
            Console.WriteLine("REQUEST: ");
            client.WriteRequest(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("RESPONSE: ");
            client.WriteResponse(Console.Out);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("READ: ");
            if (responseCreate.IsFault()) {
                Console.WriteLine(responseCreate.GetFaultString());
            } else {
                Console.WriteLine(responseCreate.GetString());
            }
        }

        public static void Main(string[] args) {
            // TestRequestXml();
            // TestResponseXml();
            // TestReadVersion();
            // TestReadRecords();
            TestCreateRecord();
            // TestSearchReadRecords();
            Console.ReadKey();
        }
    }
}
