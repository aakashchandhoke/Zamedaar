using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;

namespace Miner___Zamedaar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string chainURL;
        string postURL;
        string mineURL;

        public MainWindow()
        {
            InitializeComponent();
            chainURL = @"http://localhost:5000/chain";
            postURL = @"http://localhost:5000/transactions/new";
            mineURL = @"http://localhost:5000/mine";
        }

        private void btnMine_Click(object sender, RoutedEventArgs e)
        {
            string response = _GetRequest(mineURL);
            JObject rss = JObject.Parse(response);
            string index = (string)rss["index"];
            tbConsole.AppendText("Block with index " + index + " is added to the chain");
            tbminerid.Text = getMinerId(_GetRequest(chainURL));

        }

        public string _GetRequest(string uri)
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

        public string _PostRequest(string uri, string data, string contentType, string method = "POST")
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

        private void btnAmount_Click(object sender, RoutedEventArgs e)
        {
            int amountTotal = 0;

            JObject rss = JObject.Parse(_GetRequest(chainURL));
            JArray blocks = (JArray)rss["chain"];
            foreach (JObject arr in blocks)
            {
                JArray transactions = (JArray)arr["transactions"];
                foreach (JObject transaction in transactions)
                {
                    if ((string)transaction["recipient"] == tbminerid.Text)
                    {
                        amountTotal += (int)transaction["amount"];
                    }
                   
                }
            }
            MessageBox.Show(amountTotal.ToString());

        }
        private string getMinerId(string blockchain)
        {

            string recepient = "";

            JObject rss = JObject.Parse(_GetRequest(chainURL));
            JArray blocks = (JArray)rss["chain"];
            foreach (JObject arr in blocks)
            {
                JArray transactions = (JArray)arr["transactions"];
                foreach (JObject transaction in transactions)
                {
                    recepient = (string)transaction["recipient"];
                }
            }

            return recepient;
        }

        private void btnViewChain_Click(object sender, RoutedEventArgs e)
        {
            tbChain.Text = _GetRequest(chainURL);
        }
    }
}
