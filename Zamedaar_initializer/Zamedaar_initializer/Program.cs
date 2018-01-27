using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Zamedaar_initializer
{
    class Program
    {
        static void Main(string[] args)
        {

            string chainURL = @"http://localhost:5000/chain";
            string postURL = @"http://localhost:5000/transactions/new";
            string mineURL = @"http://localhost:5000/mine";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python";
            start.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            start.Arguments = "blockchain.py";
            Process.Start(start);

            _PostRequest(postURL, sellStringMaker("0", "b986bf873d6df900c5c0670b7dcf5b33b817da709a345176044cbc4aa4089583", "2"), "application/json; charset=utf-8");
            _PostRequest(postURL, sellStringMaker("0", "a9a5067ae9df2964fb64b0bf88537b8d2f469cd30ad747b0844fb23746458bc3", "3"), "application/json; charset=utf-8");
            _PostRequest(postURL, sellStringMaker("0", "9d1de997f3a8e71c69d9d11a6911921290b7c76f544222d4ff446f24913a7a2f", "4"), "application/json; charset=utf-8");
            _PostRequest(postURL, sellStringMaker("0", "10f6d3ce9d854d1ebfc1ca7d1981fafc122a9970093382f2c5c72cfa6ab47572", "5"), "application/json; charset=utf-8");
            _GetRequest(mineURL);
        }

        public static string _GetRequest(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static string _PostRequest(string uri, string data, string contentType, string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private static string sellStringMaker(string sender, string recepient, string landID)
        {
            return "{ \"recipient\": \"" + recepient + "\", \"sender\": \"" + sender + " \", \"amount\": " + landID + " }";

        }
    }
}
