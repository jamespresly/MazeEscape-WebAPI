using System.Security.Cryptography;
using MazeEscape.Engine.Helper;
using MazeEscape.Engine.Interfaces;
using MazeEscape.Engine.Model;

namespace MazeEscape.Engine
{
    public class MazeEncoder : IMazeEncoder
    {


        public string MazeEncode(Maze maze, string encryptionKey)
        {
            var encoded = maze.ToBase64String();

            var compressed = StringCompression.CompressString(encoded);

            var key = Convert.FromBase64String(encryptionKey);

            var iv = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            }


            var encrypted = AesEncryption.Encrypt(compressed, key, iv);

            var encryptedBase64 = Convert.ToBase64String(encrypted);
            var ivBase64 = Convert.ToBase64String(iv);

            var result = ivBase64 + encryptedBase64;

            return result;

        }

        public Maze MazeDecode(string mazeToken, string encryptionKey)
        {
            return null;
        }
    }
}
