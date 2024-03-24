using System.Text.RegularExpressions;
using MazeEscape.TestClient.DTO;
using Newtonsoft.Json;

namespace MazeEscape.TestClient
{
    internal class Program
    {
        private readonly HttpClientWrapper _clientWrapper = new();
        private readonly MazePrinter _mazePrinter = new();

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }


        private Dictionary<string, string> _commandShortcuts = new()
        {
            {"w", "player-move-forward"},
            {"a", "player-turn-left"},
            {"d", "player-turn-right"}
        };

        private void Run()
        {
            _clientWrapper.Initialise();

        
            while (true)
            {
                TryPrint();

                Console.WriteLine("Please enter next command:");
                var command = Console.ReadLine();

                var shortcut="";
                _commandShortcuts.TryGetValue(command, out shortcut);

                if(!string.IsNullOrEmpty(shortcut))
                    command = shortcut;

                var link = _clientWrapper.Root.Links.FirstOrDefault(x => x.Description == command);

                if (link != null)
                {
                    var resp = _clientWrapper.GetEndpoint(link.Href);
                    Console.WriteLine(resp);

                    continue;
                }

                var action = _clientWrapper.Root.Actions.FirstOrDefault(x => x.Description == command);

                if (action != null)
                {
                    
                    var body = Body(action, out var abort);

                    if (abort)
                    {
                        continue;
                    }


                    var resp = _clientWrapper.PostEndpoint(action.Href, body);
                    Console.WriteLine(resp);

                    continue;
                }

                Console.WriteLine("No command found with name:" + command);
                
            }
        }

        private string Body(Link action, out bool abort)
        {
            var body = JsonConvert.SerializeObject(action.Body);

            var pattern = @"\{[^{}]*\}";
            var matches = Regex.Matches(body, pattern);

            abort = false;

            foreach (Match match in matches)
            {
                if (match.Value == "{mazeToken}")
                {
                    if (_clientWrapper.MazeToken == null)
                    {
                        Console.WriteLine("You need to create a maze first");
                        abort = true;
                        break;
                    }
                    else
                    {
                        body = body.Replace("{mazeToken}", _clientWrapper.MazeToken);
                    }
                }
                else
                {
                    Console.WriteLine("Input value for parameter:" + match.Value);
                    var input = Console.ReadLine();

                    body = body.Replace(match.Value, input);
                }
            }

            return body;
        }

        private void TryPrint()
        {
            try
            {
                var position = _clientWrapper.Root.Data.position;
                if (position != null)
                {
                    try
                    {
                        _mazePrinter.PrintMaze(_clientWrapper.Root.Data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Printing error");

                        Console.WriteLine(e);
                    }
                }
            }
            catch
            {
                _mazePrinter.ClearMaze();
            }
        }
    }
}
