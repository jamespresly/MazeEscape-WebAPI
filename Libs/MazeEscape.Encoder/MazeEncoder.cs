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

        private const int EndSpacer = 10;

        private const char IsCompressedFlag = '1';
        private const char NotCompressedFlag = '0';

        private const int IVBytesLength = 16;
        private const int IVBase64Length = 24;

        public string MazeEncode(string mazeString, string encryptionKey)
        {
            var bytes = EncodeToBytes(mazeString);

            var encoded = Convert.ToBase64String(bytes);

            var uncompressed = NotCompressedFlag + encoded;

            var compressed = IsCompressedFlag + StringCompression.CompressString(encoded);            

            // sometimes compression is ineffective for small mazes
            var finalEncoded = uncompressed.Length < compressed.Length ? uncompressed : compressed;

            var key = Convert.FromBase64String(encryptionKey);
            var iv = RandomNumberGenerator.GetBytes(IVBytesLength);
            
            var encrypted = AesEncryption.Encrypt(finalEncoded, key, iv);

            var encryptedBase64 = Convert.ToBase64String(encrypted);
            var ivBase64 = Convert.ToBase64String(iv);

            var result = ivBase64 + encryptedBase64;

            return result;
        }

        public string MazeDecode(string mazeToken, string encryptionKey)
        {
            var ivBase64 = mazeToken.Substring(0, IVBase64Length);
            var encryptedBase64 = mazeToken.Substring(IVBase64Length, mazeToken.Length - IVBase64Length);

            var encrypted = Convert.FromBase64String(encryptedBase64);
            var iv = Convert.FromBase64String(ivBase64);

            var key = Convert.FromBase64String(encryptionKey);

            var decrypted = AesEncryption.Decrypt(encrypted, key, iv);

            var compressionFlag = decrypted.ElementAt(0);

            var finalEncoded = decrypted.Substring(1);

            if (compressionFlag == IsCompressedFlag)
            {
                finalEncoded = StringCompression.DecompressString(finalEncoded);
            }

            var bytes = Convert.FromBase64String(finalEncoded);

            var mazeText = DecodeFromBytes(bytes);

            return mazeText;
        }

        private byte[] EncodeToBytes(string maze)
        {
            var mazeString = maze.Replace("\r", "");

            var mazeChars = mazeString.ToCharArray();

            var bytes = new List<byte>();

            for (var i = 0; i < mazeChars.Length; i += 2)
            {
                var char1 = mazeChars[i];

                var mapped1 = _charMap[char1];
                var mapped2 = EndSpacer;

                if (i + 1 < mazeChars.Length)
                {
                    var c2 = mazeChars[i + 1];
                    mapped2 = _charMap[c2];
                }

                // pack 2 numbers into 1 byte
                var b1 = (byte)((mapped1 << 4) | mapped2);

                bytes.Add(b1);
            }

            return bytes.ToArray();
        }

        private string DecodeFromBytes(byte[] bytes)
        {
            var revCharMap = _charMap.ToDictionary(x => x.Value, x => x.Key);

            var chars = new List<char>();

            foreach (var b in bytes)
            {
                // unpack 2 numbers from 1 byte
                var num1 = (b >> 4) & 0x0F;  
                var num2 = b & 0x0F;         

                var mapped1 = revCharMap[num1];
                chars.Add(mapped1);

                if(num2 != EndSpacer)
                {
                    var mapped2 = revCharMap[num2];
                    chars.Add(mapped2);
                }
            }

            return new string(chars.ToArray()) ;
        }
    }
}
