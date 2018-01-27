using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;

namespace Zameedaar
{
    [DataContract]
    public class BlockChain
    {
        [DataContract]
        public class Block
        {
            int index;
            string previousHash;
            int proof;
            string timestamp;

            [DataContract]
            public class transaction
            {
                string landID;
                string recipient;
                string sender;
                [DataMember]
                public string LandID { get => landID; set => landID = value; }
                [DataMember]
                public string Recipient { get => recipient; set => recipient = value; }
                [DataMember]
                public string Sender { get => sender; set => sender = value; }
            };

            transaction[] transactions;
            [DataMember]
            public int Index { get => index; set => index = value; }
            [DataMember]
            public string PreviousHash { get => previousHash; set => previousHash = value; }
            [DataMember]
            public int Proof { get => proof; set => proof = value; }
            [DataMember]
            public string Timestamp { get => timestamp; set => timestamp = value; }
            [DataMember]
            public transaction[] Transactions { get => transactions; set => transactions = value; }
        };

        Block[] block;
        [DataMember]
        public Block[] Block1 { get => block; set => block = value; }

        public static BlockChain parseObject(string jsonData)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(BlockChain));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));
            stream.Position = 0;
            BlockChain dataContractDetail = (BlockChain)jsonSerializer.ReadObject(stream);
            return dataContractDetail;
        }


    }
}
