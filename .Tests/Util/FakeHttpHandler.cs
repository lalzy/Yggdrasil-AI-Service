// FakeHttpHandler.cs

namespace Yggdrasil.Tests.Util;

public class FakeHttpHandler : HttpMessageHandler
{
    public string Response { get; set; } = "";

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent(Response);
        return Task.FromResult(response);
    }
}
