using MazeEscape.Generator.Enums;
using System.Security.Cryptography;

namespace MazeEscape.Generator.Helper
{
    internal static class RandomHelper
    {
        internal static bool GetRandomChance(int i)
        {
            var random = RandomNumberGenerator.GetInt32(i);

            return random == 0;
        }
        internal static Direction GetRandomDirection()
        {
            return (Direction)RandomNumberGenerator.GetInt32(4);
        }

        internal static int GetRandomIntLessThan(int i)
        {
            return RandomNumberGenerator.GetInt32(i);
        }
    }
}
