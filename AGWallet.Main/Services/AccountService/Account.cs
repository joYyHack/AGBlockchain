using EllipticCurve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AGWallet.Main.Services.AccountService
{
    public class Account
    {
        PrivateKey PrivateKey { get; init; }
        public string PrivateKeyStr => Convert.ToHexString(PrivateKey.toString());
        PublicKey PublicKey => PrivateKey.publicKey();
        public string PublicKeyStr => Convert.ToHexString(PublicKey.toString());
        public BigInteger SecretNumber => PrivateKey.secret;
        public string Address
        {
            get
            {
                using var sha256 = SHA256.Create();
                byte[] hash = sha256.ComputeHash(PublicKey.toString());
                return Convert.ToBase64String(hash);
            }
        }

        public static Account Create()
        {
            return new Account()
            {
                PrivateKey = new()
            };
        }

        public static Account Login(string secret)
        {
            return new Account()
            {
                PrivateKey = new("secp256k1", BigInteger.Parse(secret))
            };
        }

        public string Sign(string transaction)
        {
            return Ecdsa.sign(transaction, PrivateKey).toBase64();
        }
    }
}
