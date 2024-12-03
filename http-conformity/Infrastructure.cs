using System.Net;

namespace HttpConformity;

public static class Infrastructure
{

    public static HttpClient CreateClient()
    {
        var handler = new HttpClientHandler
        {
        };

        return new HttpClient(handler);
    }

}
