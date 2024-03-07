﻿using System.Net.Http.Headers;
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

        [When(@"I make a GET request with body to:(.*) body:(.*)")]
        public void GetEndpointWithBody(string endpoint, string body)
        {
            throw new NotImplementedException();
        }

        [When(@"I make a POST request to:(.*) with body:(.*)")]
        public void PostEndpoint(string endpoint, string body)
        {
            _response = Post(endpoint, body);
        }

        [When(@"I save the mazeToken")]
        public void WhenISaveTheMazeToken()
        {
            throw new PendingStepException();
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

        [Then(@"the response contains the following:(.*)")]
        public void ThenTheResponseContainsTheFollowing(string arrayName, Table table)
        {

            var response = _response.Content.ReadAsStringAsync().Result;

            var links = JObject.Parse(response)[arrayName];

            var i = 0;

            foreach (var row in table.Rows)
            {
                var link = links[i++];

                var desc = row["description"];
                var href = row["href"];
                var method = row["method"];
                var body = row["body"];

                var linkDescription = link["description"].ToString();
                var linkHref = link["href"].ToString();
                var linkMethod = link["method"].ToString();
                var linkBody = link["body"]?.ToString(Newtonsoft.Json.Formatting.None);

                if(linkBody == null)
                    linkBody = "";

                linkDescription.Should().BeEquivalentTo(desc);
                linkHref.Should().BeEquivalentTo(href);
                linkMethod.Should().BeEquivalentTo(method);
                linkBody.Should().BeEquivalentTo(body);
            }
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
