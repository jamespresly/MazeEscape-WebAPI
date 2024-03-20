using MazeEscape.TestClient.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MazeEscape.TestClient
{
    internal class HttpClientWrapper
    {
        private HttpClient _httpClient;
        private HttpResponseMessage _response;

        public Root Root { get; set; }
        public string MazeToken { get; set; }

        public void Initialise()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5222"),
            };

            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var er = GetEndpoint("/mazes");

            Console.WriteLine(er);
        }

        public string PostEndpoint(string endpoint, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            _response = _httpClient.PostAsync(endpoint, content).Result;

            var resp = _response.Content.ReadAsStringAsync().Result;

            if (_response.IsSuccessStatusCode)
            {
                Root = JsonConvert.DeserializeObject<Root>(resp);

                MazeToken = Root.data.mazeToken;
            }

            return resp;
        }

        public string GetEndpoint(string endpoint)
        {
            _response = _httpClient.GetAsync(endpoint).Result;

            var resp = _response.Content.ReadAsStringAsync().Result;

            if (_response.IsSuccessStatusCode)
            {
                Root = JsonConvert.DeserializeObject<Root>(resp);
            }

            return resp;
        }
    }
}
