using System.Security.Cryptography;
using MazeEscape.Encoder.Helper;
using MazeEscape.Encoder.Interfaces;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Engine.Model;

namespace MazeEscape.Encoder
{
    public class MazeEncoder : IMazeEncoder
    {
        private readonly IMazeConverter _mazeConverter;

        

        public MazeEncoder(IMazeConverter mazeConverter)
        {
            _mazeConverter = mazeConverter;
        }

        public string MazeEncode(Maze maze, string encryptionKey)
        {
            var encoded = ToBase64String(maze);

            var compressed = StringCompression.CompressString(encoded);

            var key = Convert.FromBase64String(encryptionKey);

            var iv = RandomNumberGenerator.GetBytes(16);
            
            var encrypted = AesEncryption.Encrypt(compressed, key, iv);

            var encryptedBase64 = Convert.ToBase64String(encrypted);
            var ivBase64 = Convert.ToBase64String(iv);

            var result = ivBase64 + encryptedBase64;

            return result;

        }

        public Maze MazeDecode(string mazeToken, string encryptionKey)
        {
            var ivBase64 = mazeToken.Substring(0, 24);
            var encryptedBase64 = mazeToken.Substring(24, mazeToken.Length - 24);

            var encrypted = Convert.FromBase64String(encryptedBase64);
            var iv = Convert.FromBase64String(ivBase64);

            var key = Convert.FromBase64String(encryptionKey);

            var decrypted = AesEncryption.Decrypt(encrypted, key, iv);


            var decompressed = StringCompression.DecompressString(decrypted);

            var mazText= FromBase64String(decompressed);

            var maze = _mazeConverter.GenerateFromText(mazText);

            return maze;

        }

        public List<string> GetPresets(string path)
        {
            var presets = Directory.EnumerateFiles(path + "\\Presets");

            return presets.ToList();
        }



        private readonly Dictionary<char, int> _charMap = 
            new Dictionary<char, int>()
            {
                {'+', 0},
                {' ', 1},
                {'\n', 2},
                {'E',4},
                {'\u25b2', 5},
                {'\u25ba', 6},
                {'\u25bc', 7},
                {'\u25c4', 8},
            };

     

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

        private string ToBase64String(Maze maze)
        {
            var mazeString = _mazeConverter.ToText(maze).Replace("\r", "");

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
