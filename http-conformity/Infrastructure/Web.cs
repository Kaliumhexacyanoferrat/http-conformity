using System.Net;

namespace HttpConformity.Infrastructure;

public static class Web
{

    public static HttpClient CreateClient()
    {
        var handler = new HttpClientHandler
        {

        };

        return new HttpClient(handler);
    }

}
