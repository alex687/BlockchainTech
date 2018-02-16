using System;
using Node.Requests;

namespace Wallet
{
    class Program
    {
        private static WalletManager _walletManager;
        private static NodeCommunicator _nodeCommunicator;
        static void Main(string[] args)
        {
            Console.WriteLine("Node url:");
            var nodeUrl = Console.ReadLine();
            _nodeCommunicator = new NodeCommunicator(nodeUrl);

            string input = string.Empty;
            while (!input.ToLower().Equals("exit"))
            {
                Console.Write(
                    "Enter operation [\"Create\", \"Load\", \"Balance\", \"Receive\", \"Send\", \"Exit\"]: ");
                input = Console.ReadLine().ToLower().Trim();

                switch (input)
                {
                    case "create":
                        CreateWallet();
                        break;

                    case "load":
                        LoadWallet();
                        break;

                    case "balance":
                        ShowBalance();
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

        private static void LoadWallet()
        {
            Console.WriteLine("Enter wallet name");
            var name = Console.ReadLine();
            _walletManager = WalletManager.LoadWallet(name);

            Console.WriteLine("Loaded");
        }

        private static void ShowBalance()
        {
            var balance =  _nodeCommunicator.GetBalance(_walletManager.GetAddress(), 0).Result;

            Console.WriteLine(balance);
        }


        private static void Receive()
        {
            Console.WriteLine($"Address Recieve {_walletManager.GetAddress()}");
        }

        private static void Send()
        {
            Console.WriteLine("To Address:");
            var to = Console.ReadLine();

            Console.WriteLine("Ammount:");
            var ammount = decimal.Parse(Console.ReadLine());

            var pendingTransaction = new PendingTransactionRequest
            {
                SenderPublickKey = _walletManager.GetPublicKey(),
                From = _walletManager.GetAddress(),
                To = to,
                Amount = ammount
            };

            _walletManager.SignTransactionRequest(pendingTransaction);
            _walletManager.SetTransactionRequestHash(pendingTransaction);

            var response = _nodeCommunicator.PublishTransaction(pendingTransaction).Result;

            Console.WriteLine("Transaction send:");
            Console.WriteLine(response.IsSuccessStatusCode);
        }

        private static void CreateWallet()
        {
            Console.WriteLine("Enter wallet name");
            var name = Console.ReadLine();

            _walletManager = new WalletManager(name);
        }
    }
}
