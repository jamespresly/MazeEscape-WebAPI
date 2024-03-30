using System.Security.Cryptography;
using MazeEscape.Encoder.Helper;
using MazeEscape.Encoder.Interfaces;
using MazeEscape.Model.Constants;

namespace MazeEscape.Encoder
{
    public class MazeEncoder : IMazeEncoder
    {

        private readonly Dictionary<char, int> _charMap = new()
        {
            { MazeChars.Wall, 0},
            { MazeChars.Corridor , 1},
            { MazeChars.LineSeparator, 2},
            { MazeChars.Exit,4},
            { MazeChars.PlayerStart,5},

            { MazeChars.UpArrow , 6},
            { MazeChars.RightArrow, 7},
            { MazeChars.DownArrow , 8},
            { MazeChars.LeftArrow, 9},
        };

 

        public string MazeEncode(string mazeString, string encryptionKey)
        {
            var encoded = ToBase64String(mazeString);

            var compressed = StringCompression.CompressString(encoded);

            var key = Convert.FromBase64String(encryptionKey);

            var iv = RandomNumberGenerator.GetBytes(16);
            
            var encrypted = AesEncryption.Encrypt(compressed, key, iv);

            var encryptedBase64 = Convert.ToBase64String(encrypted);
            var ivBase64 = Convert.ToBase64String(iv);

            var result = ivBase64 + encryptedBase64;

            return result;
        }

        public string MazeDecode(string mazeToken, string encryptionKey)
        {
            var ivBase64 = mazeToken.Substring(0, 24);
            var encryptedBase64 = mazeToken.Substring(24, mazeToken.Length - 24);

            var encrypted = Convert.FromBase64String(encryptedBase64);
            var iv = Convert.FromBase64String(ivBase64);

            var key = Convert.FromBase64String(encryptionKey);

            var decrypted = AesEncryption.Decrypt(encrypted, key, iv);

            var decompressed = StringCompression.DecompressString(decrypted);

            var mazeText= FromBase64String(decompressed);

            return mazeText;
        }

        private string FromBase64String(string base64String)
        {
            var bytes = Convert.FromBase64String(base64String);

            var revCharMap = _charMap.ToDictionary(x => x.Value, x => x.Key);

            var chars = new List<char>();

            foreach (var b in bytes)
            {
                var i = Convert.ToInt32(b);

                var mapped = revCharMap[i];

                chars.Add(mapped);
            }

            return new string(chars.ToArray()) ;
        }

        private string ToBase64String(string maze)
        {
            var mazeString = maze.Replace("\r", "");

            var mazeChars = mazeString.ToCharArray();

            var bytes = new byte[mazeChars.Length];

            for (var i = 0; i < mazeChars.Length; i++)
            {
                var c = mazeChars[i];

                var enc = _charMap[c];
                
                bytes[i] = Convert.ToByte(enc);
            }

            var base64String = Convert.ToBase64String(bytes);

            return base64String;
        }






    }
}
