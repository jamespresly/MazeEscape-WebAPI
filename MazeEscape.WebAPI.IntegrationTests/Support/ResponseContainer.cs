namespace MazeEscape.WebAPI.IntegrationTests.Support;

public class ResponseContainer
{
    public HttpResponseMessage HttpResponse { get; private set; }
    public string ResponseString { get; private set; }

    public void SetHttpResponse(HttpResponseMessage response)
    {
        HttpResponse = response;
        ResponseString = HttpResponse.Content.ReadAsStringAsync().Result;
    }
}