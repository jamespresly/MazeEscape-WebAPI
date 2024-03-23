using MazeEscape.WebAPI.IntegrationTests.Support;
using Newtonsoft.Json.Linq;

namespace MazeEscape.WebAPI.IntegrationTests.StepDefinitions;

[Binding]
public sealed class HypermediaStepDefinitions
{
    private readonly ResponseContainer _responseContainer;

    public HypermediaStepDefinitions(ResponseContainer responseContainer)
    {
        _responseContainer = responseContainer;
    }

    [Then(@"the response contains the following hypermedia array:(.*) with values:")]
    public void ResponseContainsTheFollowingHypermediaArrayWithValues(string arrayName, Table table)
    {
        var response = _responseContainer.ResponseString;

        var links = JObject.Parse(response)[arrayName];

        var linksCount = links?.Count() ?? 0;

        linksCount.Should().Be(table.RowCount);

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

            if (linkBody == null)
                linkBody = "";

            linkDescription.Should().BeEquivalentTo(desc);
            linkHref.Should().BeEquivalentTo(href);
            linkMethod.Should().BeEquivalentTo(method);
            linkBody.Should().BeEquivalentTo(body);
        }
    }
}