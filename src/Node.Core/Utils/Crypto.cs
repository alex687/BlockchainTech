using System.Text;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;

namespace Node.Core.Utils
{
    public static class Crypto
    {
        public static bool VerifySignature(string publicKey, string dataToSign, string signature)
        {
            byte[] inputData = Encoding.UTF8.GetBytes(dataToSign);

            var publicKeyParam = KeyFromPublic(publicKey);

            var signer = SignerUtilities.GetSigner("ECDSA");
            signer.Init(false, publicKeyParam);
            signer.BlockUpdate(inputData, 0, inputData.Length);

            return signer.VerifySignature(Hex.Decode(signature));
        }

        private static ECPublicKeyParameters KeyFromPublic(string publicKey)
        {
            X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
            ECDomainParameters domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

            ECPublicKeyParameters key = new ECPublicKeyParameters("ECDSA", curve.Curve.DecodePoint(Hex.Decode(publicKey)), domain);
            return key;
        }

    }
}
