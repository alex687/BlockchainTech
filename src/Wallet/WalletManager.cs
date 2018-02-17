using System.IO;
using System.Text;
using Node.Core.Crypto;
using Node.Requests;

namespace Wallet
{
    public class WalletManager
    {
        private readonly string _name;
        private readonly string _privateKey;
        private readonly string _publicKey;

        public WalletManager(string name)
            : this(name, Crypto.GeneratePrivateKey())
        {
        }

        public WalletManager(string name, string privateKey)
        {
            _name = TrimName(name);
            _privateKey = privateKey;
            _publicKey = Crypto.GetPublicKey(_privateKey);
        }

        public static WalletManager LoadWallet(string name)
        {
            name = TrimName(name);
            string privateKey = File.ReadAllText($"wallet/{name}_private_key", Encoding.UTF8);

            return new WalletManager(name, privateKey);
        }

        public void SaveWallet()
        {
            string path = Path.Combine("wallet");
            string privateKeyFilePath = Path.Combine(path, $"{_name}_private_key");

            Directory.CreateDirectory(path);

            if (File.Exists(privateKeyFilePath))
            {
                return;
            }

            using (FileStream fileStream = File.Create(privateKeyFilePath))
            using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
            {
                writer.Write(_privateKey);
            }
        }

        public void SignTransactionRequest(PendingTransactionRequest pendingTransactionRequest)
        {
            var dataForSign = pendingTransactionRequest.From
                              + pendingTransactionRequest.To
                              + pendingTransactionRequest.Amount;

            pendingTransactionRequest.SenderSignature = Crypto.GetSignature(dataForSign, _privateKey);
        }

        public void SetTransactionRequestHash(PendingTransactionRequest pendingTransactionRequest)
        {
            pendingTransactionRequest.Hash = Hash.ComputeHash(
                 pendingTransactionRequest.From,
                 pendingTransactionRequest.To,
                 pendingTransactionRequest.SenderPublickKey,
                 pendingTransactionRequest.SenderSignature,
                 pendingTransactionRequest.Amount.ToString()
             );
        }

        public string GetPublicKey()
        {
            return _publicKey;
        }

        public string GetAddress()
        {
            return _publicKey.ComputeRipeMd160Hash();
        }

        private static string TrimName(string name)
        {
            return name.Trim(' ', '_');
        }
    }
}
