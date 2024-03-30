namespace MazeEscape.GeneratorDemo.Helper
{
    internal class ConsoleHelper
    {
        public void InitialiseConsole(ConsoleColor backgroundColour, ConsoleColor borderColour)
        {
            Console.BackgroundColor = backgroundColour;
            Console.ForegroundColor = borderColour;
        }

        internal void WriteFirstFrame(string first)
        {
            Console.Clear();
            Console.WriteLine(first);
        }

        internal void PromptUser()
        {
            var lastRow = Console.WindowHeight - 2;
            Console.SetCursorPosition(0, lastRow);
            Console.WriteLine("press any key to continue...");
            Console.ReadKey();
        }

        internal void WriteDiffsToConsole(List<string> allStrings, int delay, ConsoleColor color = ConsoleColor.White)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = color;

            var first = allStrings[0];

            var currentString = first.Split("\n");

            for (var stringCount = 0; stringCount < allStrings.Count; stringCount++)
            {
                var nextString = allStrings[stringCount].Split("\n");

                for (int line = 0; line < nextString.Length; line++)
                {
                    if (currentString[line] != nextString[line])
                    {
                        var currentChars = currentString[line].ToCharArray();
                        var nextChars = nextString[line].ToCharArray();

                        for (int ch = 0; ch < nextChars.Length; ch++)
                        {
                            if (currentChars[ch] != nextChars[ch])
                            {
                                Console.SetCursorPosition(ch, line);
                                Console.Write(nextChars[ch]);
                            }
                        }
                    }
                }

                currentString = nextString;

                if (delay > 0)
                {
                    Thread.Sleep(delay);
                }
            }

            Console.ResetColor();
        }
    }
}
