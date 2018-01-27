using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Codeplex.Data;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;

namespace Zameedaar
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
            postURL =  @"http://localhost:5000/transactions/new";
            mineURL =  @"http://localhost:5000/mine";
        }

        private void btnMakeTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string jsonObject = sellStringMaker(tbSender.Text, tbReceiver.Text, tbLandID.Text);
                string returnValue = _PostRequest(postURL, jsonObject, "application/json; charset=utf-8");
                MessageBox.Show(returnValue);
            }
            
            catch
            {
                MessageBox.Show("Error");
            }
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

        private void btntest_Click(object sender, RoutedEventArgs e)
        {
            string jsonData = File.ReadAllText(@"C:\Users\Shubham\Desktop\chain.txt");
            BlockChain chain = BlockChain.parseObject(jsonData);
            MessageBox.Show(chain.Block1[1].Index.ToString());

        }

        private void btntest2_Click(object sender, RoutedEventArgs e)
        {
            WebClient webClient = new WebClient();
            //dynamic result = JsonValue.Parse(webClient.DownloadString("https://api.foursquare.com/v2/users/self?oauth_token=XXXXXXX"));
            //dynamic result = JsonValue.Parse(File.ReadAllText(@"C:\Users\Shubham\Desktop\chain.txt"));

            //Console.WriteLine(result.response.user.firstName);
        }

        private void btntest3_Click(object sender, RoutedEventArgs e)
        {
            JObject rss = JObject.Parse(File.ReadAllText(@"C:\Users\Shubham\Desktop\chain.txt"));
            //string check = (string) rss["chain"][1]["index"];


            JArray blocks = (JArray)rss["chain"];

            MessageBox.Show(blocks.Count.ToString());
        }


        private Boolean IsOwner(string ownerID, string landID, string blockchain)
        {
            List<string> owners = new List<string>();
            JObject rss = JObject.Parse(blockchain);
            JArray blocks = (JArray)rss["chain"];
            foreach (JObject arr in blocks)
            {
                JArray transactions = (JArray)arr["transactions"];
                foreach (JObject transaction in transactions)
                {
                    if (landID.Equals((string) transaction["amount"]) == true)
                    {
                        owners.Add((string) transaction["recipient"]);
                    }
                }
            }
            if (owners.ElementAt(owners.Count - 1).Equals(ownerID))
            {
                return true;
            }

            return false;
        }

        private string OwnerOf(string landID, string blockchain)
        {

            List<string> owners = new List<string>();
            JObject rss = JObject.Parse(blockchain);
            JArray blocks = (JArray)rss["chain"];
            foreach (JObject arr in blocks)
            {
                JArray transactions = (JArray)arr["transactions"];
                foreach (JObject transaction in transactions)
                {
                    if (landID.Equals((string)transaction["amount"]) == true)
                    {
                        owners.Add((string)transaction["recipient"]);
                        // MessageBox.Show("Index : " + (string)arr["index"] + " Amount : " + (string)transaction["amount"] + " Sender : " + (string)transaction["sender"] + " Recipient : " + (string)transaction["recipient"]);

                    }
                }
            }
            return owners.ElementAt(owners.Count - 1);
        }

        private List<string> OwnerLands(string ownerID, string blockchain)
        {

            List<string> lands = new List<string>();
            JObject rss = JObject.Parse(blockchain);
            JArray blocks = (JArray)rss["chain"];
            foreach (JObject arr in blocks)
            {
                JArray transactions = (JArray)arr["transactions"];
                foreach (JObject transaction in transactions)
                {
                    lands.Add((string)transaction["amount"]);
                }
            }
            List<string> ownerLand = new List<string>();
            string[] landsD = lands.ToArray().Distinct().ToArray();
            foreach(string land in landsD)
            {
                if (OwnerOf(land, blockchain) == ownerID)
                {
                    ownerLand.Add(land);
                }
            }
            // 2d array banaoo
            return ownerLand;
        }


        private Boolean temp(string landID, string blockchain)
        {
            string result = "";

            JObject rss = JObject.Parse(blockchain);
            JArray blocks = (JArray)rss["chain"];
            foreach(JObject arr in blocks)
            {
                JArray transactions = (JArray)arr["transactions"];
                foreach (JObject transaction in transactions)
                {
                    result += (transaction["amount"] + " -------- " + transaction["recipient"] + " ----------- " + transaction["sender"] + Environment.NewLine);
                }
            }
            MessageBox.Show(result);
            return true;

        }

        private void btntest4_Click(object sender, RoutedEventArgs e)
        {
            string own = OwnerOf("7", _GetRequest(chainURL));
            MessageBox.Show(own);
        }

        private void btnGetChain_Click(object sender, RoutedEventArgs e)
        {
            tbConsole.AppendText("\n" + _GetRequest(chainURL));
        }

        private string sellStringMaker(string sender, string recepient, string landID)
        {
            return "{ \"recipient\": \"" + recepient + "\", \"sender\": \"" + sender + " \", \"amount\": " + landID + " }";

        }

        private void btnSenderSet_Click(object sender, RoutedEventArgs e)
        {
            tbLandID.Items.Clear();
            string blockChainText = _GetRequest(chainURL);
            List<string> lands =  OwnerLands(tbSender.Text, blockChainText);
            foreach(string land in lands)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = land;
                tbLandID.Items.Add(cbi);
            }


        }
    }
}
