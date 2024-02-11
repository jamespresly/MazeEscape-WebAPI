using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;


namespace MazeEscape.WebAPI.IntegrationTests.StepDefinitions
{
    [Binding]
    public sealed class MazeEscapeStepDefinitions
    {
        private HttpClient _httpClient;
        private HttpResponseMessage _response;


        [Given(@"the MazeEscape client is running")]
        public void GivenIAmAClient()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5222"),
            };

            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [When(@"I make a GET request to:(.*)")]
        public void GetEndpoint(string endpoint)
        {
            _response = Get(endpoint);
        }

        [When(@"I make a POST request to:(.*) with body:(.*)")]
        public void PostEndpoint(string endpoint, string body)
        {
            _response = Post(endpoint, body);
        }

        [Then(@"the status code is:(.*)")]
        public void GetResponse(string statusCode)
        {
            Console.WriteLine(statusCode);
            Console.WriteLine(_response.ReasonPhrase);

            _response.StatusCode.ToString().Should().Be(statusCode);
        }

        [Then(@"the response data is an array which contains value:(.*)")]
        public void CheckDataIsListContaining(string value)
        {
            var arr = JArray.Parse(_response.Content.ReadAsStringAsync().Result);

            var results = arr.ToObject<List<string>>();

            results.Should().NotBeNull();
            results.Should().Contain(value);

        }

        [Then(@"the response data is an object which contains non-null value:(.*)")]
        public void CheckDataIsObjectContaining(string value)
        {
            var obj = JObject.Parse(_response.Content.ReadAsStringAsync().Result);

            obj.Should().NotBeNull();
            obj.Should().Contain(c => c.Key == value);

            obj.Value<string>(value).Should().NotBeNullOrEmpty();

        }

        [Then(@"the response message is:(.*)")]
        public void TheResponseMessageIs(string value)
        {
            var resp = _response.Content.ReadAsStringAsync().Result;
            resp.Should().Contain(value);
        }


        private HttpResponseMessage Get(string endpoint)
        {
            var response = _httpClient.GetAsync(endpoint).Result;

            var uri = response.RequestMessage?.RequestUri?.ToString();
            Console.WriteLine(uri);

            return response;
        }

        private HttpResponseMessage Post(string endpoint, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsync(endpoint, content).Result;

            var uri = response.RequestMessage?.RequestUri?.ToString();
            Console.WriteLine(uri);

            var payload = response.RequestMessage?.Content?.ReadAsStringAsync().Result;
            Console.WriteLine(payload);

            return response;
        }

    }
}
