using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Helper
{
    public class Encryption
    {
        UTF8Encoding _enc;
        RijndaelManaged _rcipher;
        byte[] _key, _pwd, _ivBytes, _iv;

        /***
		 * Encryption mode enumeration
		 */
        private enum EncryptMode { ENCRYPT, DECRYPT };

        static readonly char[] CharacterMatrixForRandomIVStringGeneration = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
        };

        public static string GenerateRandomIV(int length)
        {
            char[] _iv = new char[length];
            byte[] randomBytes = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes); //Fills an array of bytes with a cryptographically strong sequence of random values. 
            }

            for (int i = 0; i < _iv.Length; i++)
            {
                int ptr = randomBytes[i] % CharacterMatrixForRandomIVStringGeneration.Length;
                _iv[i] = CharacterMatrixForRandomIVStringGeneration[ptr];
            }

            return new string(_iv);
        }

        public Encryption()
        {
            _enc = new UTF8Encoding();
            _rcipher = new RijndaelManaged();
            _rcipher.Mode = CipherMode.CBC;
            _rcipher.Padding = PaddingMode.PKCS7;
            _rcipher.KeySize = 256;
            _rcipher.BlockSize = 128;
            _key = new byte[32];
            _iv = new byte[_rcipher.BlockSize / 8]; //128 bit / 8 = 16 bytes
            _ivBytes = new byte[16];
        }

        private String encryptDecrypt(string _inputText, string _encryptionKey, EncryptMode _mode, string _initVector)
        {

            string _out = "";// output string
                             //_encryptionKey = MD5Hash (_encryptionKey);
            _pwd = Encoding.UTF8.GetBytes(_encryptionKey);
            _ivBytes = Encoding.UTF8.GetBytes(_initVector);

            int len = _pwd.Length;
            if (len > _key.Length)
            {
                len = _key.Length;
            }
            int ivLenth = _ivBytes.Length;
            if (ivLenth > _iv.Length)
            {
                ivLenth = _iv.Length;
            }

            Array.Copy(_pwd, _key, len);
            Array.Copy(_ivBytes, _iv, ivLenth);
            _rcipher.Key = _key;
            _rcipher.IV = _iv;

            if (_mode.Equals(EncryptMode.ENCRYPT))
            {
                //encrypt
                byte[] plainText = _rcipher.CreateEncryptor().TransformFinalBlock(_enc.GetBytes(_inputText), 0, _inputText.Length);
                _out = Convert.ToBase64String(plainText);
            }
            if (_mode.Equals(EncryptMode.DECRYPT))
            {
                //decrypt
                byte[] plainText = _rcipher.CreateDecryptor().TransformFinalBlock(Convert.FromBase64String(_inputText), 0, Convert.FromBase64String(_inputText).Length);
                _out = _enc.GetString(plainText);
            }
            _rcipher.Dispose();
            return _out;// return encrypted/decrypted string
        }

        public string encrypt(string _plainText, string _key, string _iv)
        {
            return remove_ext(encryptDecrypt(_plainText, _key, EncryptMode.ENCRYPT, _iv));
        }

        public string decrypt(string _encryptedText, string _key, string _initVector)
        {
            return encryptDecrypt(add_ext(_encryptedText), _key, EncryptMode.DECRYPT, _initVector);
        }

        public static string getHashSha256(string text, int length)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x); //covert to hex string
            }
            if (length > hashString.Length)
                return hashString;
            else
                return hashString.Substring(0, length);
        }

        //this function is no longer used.
        private static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }
            Console.WriteLine("md5 hash of they key=" + strBuilder.ToString());
            return strBuilder.ToString();
        }

        private String remove_ext(String str)
        {
            str = str.Replace("/", "Pq2SZ6T");
            str = str.Replace("+", "L6r2EsQ");
            str = str.Replace("=", "K5SZsw6");
            str = str.Replace("\n", "");
            return str;
        }

        private String add_ext(String str)
        {
            str = str.Replace("Pq2SZ6T", "/");
            str = str.Replace("L6r2EsQ", "+");
            str = str.Replace("K5SZsw6", "=");
            return str;
        }

        public static string GenerateAlarmUId()
        {
            int length = 12;
            char[] _iv = new char[length];
            byte[] randomBytes = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            for (int i = 0; i < _iv.Length; i++)
            {
                int ptr = randomBytes[i] % CharacterMatrixForRandomIVStringGeneration.Length;
                _iv[i] = CharacterMatrixForRandomIVStringGeneration[ptr];
            }
            return new string(_iv);
        }
    }
}
