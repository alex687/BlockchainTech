using System;
using System.IO;
using System.Text;
using Node.Core.Crypto;
using Node.Requests;

namespace Wallet
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = string.Empty;
            while (!input.ToLower().Equals("exit"))
            {
                Console.Write(
                    "Enter operation [\"Create\", \"Recover\", \"Balance\", \"History\", \"Receive\", \"Send\", \"Exit\"]: ");
                input = Console.ReadLine().ToLower().Trim();

                switch (input)
                {
                    case "create":
                        CreateWallet();
                        break;

                    case "recover":
                        RecoverWallet();
                        break;

                    case "balance":
                        ShowBalance();
                        break;

                    case "history":
                        ShowHistory();
                        break;

                    case "send":
                        Send();
                        break;

                    case "receive":
                        Receive();
                        break;
                }
            }
        }

        private static void RecoverWallet()
        {
            throw new NotImplementedException();
        }

        private static void ShowBalance()
        {
            throw new NotImplementedException();
        }

        private static void ShowHistory()
        {
            throw new NotImplementedException();
        }

        private static void Receive()
        {
            throw new NotImplementedException();
        }

        private static void Send()
        {
            var privateKey = LoadPrivateKey();
              string publicKey = Crypto.GetPublicKey(privateKey);

            Console.WriteLine(publicKey);
            Console.WriteLine(publicKey.ComputeRipeMd160Hash());


            var t = new PendingTransactionRequest
            {
                From = publicKey.ComputeRipeMd160Hash(),
                To = "Ti si tiha",
                SenderPublickKey = publicKey,
                Amount = 50
            };

            var dataForSign = t.From + t.To + t.Amount;

            t.SenderSignature =Crypto.GetSignature(dataForSign, privateKey);

            t.Hash = Hash.ComputeHash(t.From, t.To, t.SenderPublickKey,
                t.SenderSignature, t.Amount.ToString());


            Console.WriteLine("Transaction !!!!");
            Console.WriteLine(t.From);
            Console.WriteLine(t.To);
            Console.WriteLine(t.SenderPublickKey);
            Console.WriteLine(t.Amount);
            Console.WriteLine(t.SenderSignature);
            Console.WriteLine(t.Hash);

        }

        private static string LoadPrivateKey()
        {
            string privateKey = File.ReadAllText($"wallet/private_key", Encoding.UTF8);
            return privateKey.Trim();
        }

        private static void CreateWallet()
        {
            string path = Path.Combine("wallet");
            string privateKeyFilePath = Path.Combine(path, "private_key");

            // Create the output directory if it does not exist already.
            Directory.CreateDirectory(path);

            // Let's not overwrite existing keys.
            if (File.Exists(privateKeyFilePath))
            {
                return;
            }

            string newPrivateKey = Crypto.GeneratePrivateKey();

            // NOTE: We use the nodes port to distiguish between different nodes running locally.
            using (FileStream fileStream = File.Create(privateKeyFilePath))
            using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
            {
                writer.Write(newPrivateKey);
            }
        }
    }
}
