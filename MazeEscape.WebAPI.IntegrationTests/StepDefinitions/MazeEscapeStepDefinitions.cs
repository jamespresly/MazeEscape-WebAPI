using System.Net.Http.Headers;
using System.Text;
using MazeEscape.WebAPI.IntegrationTests.Support;
using Newtonsoft.Json.Linq;


namespace MazeEscape.WebAPI.IntegrationTests.StepDefinitions
{
    [Binding]
    public sealed class MazeEscapeStepDefinitions
    {
        private readonly ResponseContainer _responseContainer;

        private HttpClient _httpClient;

        private string _mazeToken;


        public MazeEscapeStepDefinitions(ResponseContainer responseContainer)
        {
            _responseContainer = responseContainer;
        }


        [Given(@"the MazeEscape client is running")]
        public void MazeClientIsRunning()
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
            var response = Get(endpoint);
            _responseContainer.SetHttpResponse(response);
        }

        [When(@"I make a POST request to:(.*) with body:(.*)")]
        public void PostEndpoint(string endpoint, string body)
        {
            var response = Post(endpoint, body);
            _responseContainer.SetHttpResponse(response);
        }

        [When(@"I make a POST request to:(.*) with saved mazeToken and body:(.*)")]
        public void PostEndpointWithMazeToken(string endpoint, string body)
        {
            body = body.Replace("{mazeToken}", _mazeToken);

            var response = Post(endpoint, body);
            _responseContainer.SetHttpResponse(response);
        }

        [When(@"I save the mazeToken")]
        public void SaveTheMazeToken()
        {
            var obj = JObject.Parse(_responseContainer.ResponseString);

            _mazeToken = obj["data"]["mazeToken"].ToString();
        }


        [Then(@"the status code is:(.*)")]
        public void StatusCodeIs(string statusCode)
        {
            Console.WriteLine(statusCode);
            Console.WriteLine(_responseContainer.HttpResponse.ReasonPhrase);

            _responseContainer.HttpResponse.StatusCode.ToString().Should().Be(statusCode);
        }

        [Then(@"the response data is an array which contains value:(.*)")]
        public void ResponseDataContainsArrayByName(string value)
        {
            var obj = JObject.Parse(_responseContainer.ResponseString);

            var data = obj["data"].ToString();

            var arr = JArray.Parse(data);

            var results = arr.ToObject<List<string>>();

            results.Should().NotBeNull();
            results.Should().Contain(value);
        }

        [Then(@"the response data contains a non-null variable named:(.*)")]
        public void ResponseDataContainsNonNullVariableByName(string name)
        {
            var obj = JObject.Parse(_responseContainer.ResponseString);

            var data = JObject.Parse(obj["data"].ToString());

            data.Should().NotBeNull();
            data.Should().Contain(c => c.Key == name);

            data.Value<string>(name).Should().NotBeNullOrEmpty();
        }

        [Then(@"the response data contains an int named:(.*) with value:(.*)")]
        public void ResponseDataContainsVariableByName(string name, int value)
        {
            var obj = JObject.Parse(_responseContainer.ResponseString);

            var data = JObject.Parse(obj["data"].ToString());

            data.Should().NotBeNull();
            data.Should().Contain(c => c.Key == name);

            data.Value<int>(name).Should().Be(value);
        }

        [Then(@"the response contains error message:(.*)")]
        public void ResponseContainsErrorMessage(string value)
        {
            var obj = JObject.Parse(_responseContainer.ResponseString);

            var error = obj["error"].ToString();

            error.Should().NotBeNull();
            error.Should().Contain(value);
        }

        [Then(@"the response message contains:(.*)")]
        public void ResponseMessageContains(string value)
        {
            var response = _responseContainer.ResponseString;
            response.Should().Contain(value);
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
