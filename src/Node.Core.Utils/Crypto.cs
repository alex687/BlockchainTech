﻿using System.Text;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;

namespace Node.Core.Crypto
{
    public static class Crypto
    {
      public static string GeneratePrivateKey()
        {
            ECKeyPairGenerator gen = new ECKeyPairGenerator();
            SecureRandom secureRandom = new SecureRandom();
            X9ECParameters ps = SecNamedCurves.GetByName("secp256k1");
            ECDomainParameters ecParams = new ECDomainParameters(ps.Curve, ps.G, ps.N, ps.H);
            ECKeyGenerationParameters keyGenParam = new ECKeyGenerationParameters(ecParams, secureRandom);
            gen.Init(keyGenParam);

            AsymmetricCipherKeyPair keyPair = gen.GenerateKeyPair();
            ECPrivateKeyParameters privateKey = (ECPrivateKeyParameters)keyPair.Private;

            byte[] privateKeyBytes = privateKey.D.ToByteArrayUnsigned();
            return ByteArrayToString(privateKeyBytes);
        }

        public static string GetSignature(string dataToSign, string privateKey)
        {
            AsymmetricCipherKeyPair key = KeyFromPrivate(privateKey);

            byte[] inputData = Encoding.UTF8.GetBytes(dataToSign);

            ISigner signer = SignerUtilities.GetSigner("ECDSA");
            signer.Init(true, key.Private);
            signer.BlockUpdate(inputData, 0, inputData.Length);

            return Hex.ToHexString(signer.GenerateSignature());
        }

        public static string GetPublicKey(string privateKey)
        {
            BigInteger privateKeyInt = new BigInteger(privateKey, 16);

            X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
            ECDomainParameters domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

            ECPoint q = domain.G.Multiply(privateKeyInt);
            byte[] bytes = q.GetEncoded();

            return Hex.ToHexString(bytes);
        }

        public static bool VerifySignature(string publicKey, string dataToSign, string signature)
        {
            byte[] inputData = Encoding.UTF8.GetBytes(dataToSign);

            ECPublicKeyParameters publicKeyParam = KeyFromPublic(publicKey);

            ISigner signer = SignerUtilities.GetSigner("ECDSA");
            signer.Init(false, publicKeyParam);
            signer.BlockUpdate(inputData, 0, inputData.Length);

            return signer.VerifySignature(Hex.Decode(signature));
        }

        private static AsymmetricCipherKeyPair KeyFromPrivate(string privateKey)
        {
            BigInteger privateKeyInt = new BigInteger(privateKey, 16);

            X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
            ECDomainParameters domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

            ECPoint q = domain.G.Multiply(privateKeyInt);
            ECPrivateKeyParameters privateKeyParam = new ECPrivateKeyParameters(privateKeyInt, domain);
            ECPublicKeyParameters publicKeyParam = new ECPublicKeyParameters(q, domain);

            return new AsymmetricCipherKeyPair(publicKeyParam, privateKeyParam);
        }

        private static ECPublicKeyParameters KeyFromPublic(string publicKey)
        {
            X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
            ECDomainParameters domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

            ECPublicKeyParameters key = new ECPublicKeyParameters("ECDSA", curve.Curve.DecodePoint(Hex.Decode(publicKey)), domain);
            return key;
        }

        private static string ByteArrayToString(byte[] bytes)
        {
            string str = string.Empty;
            foreach (byte x in bytes)
            {
                str += string.Format("{0:x2}", x);
            }

            return str;
        }
    }
}
