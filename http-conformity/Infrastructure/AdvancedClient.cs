using System.Net.Sockets;

namespace HttpConformity.Infrastructure;

public class AdvancedClient : IAsyncDisposable
{
    private readonly TcpClient _client;

    private readonly Uri _uri;

    public string Path
    {
        get
        {
            var path = _uri.AbsolutePath;
            return (string.IsNullOrEmpty(path)) ? "/" : path;
        }
    }

    public AdvancedClient(Uri uri)
    {
        _uri = uri;
        _client = new TcpClient(uri.Host, uri.Port);
    }

    public async Task WriteAsync(string content)
    {
        var stream = _client.GetStream();

        await using var writer = new StreamWriter(stream, leaveOpen: true);

        await writer.WriteAsync(content);

        await writer.FlushAsync();
    }

    public async Task<string> ReadToEndAsync()
    {
        var stream = _client.GetStream();

        using var reader = new StreamReader(stream, leaveOpen: true);

        return await reader.ReadToEndAsync();
    }

    public ValueTask DisposeAsync()
    {
        _client.Dispose();
        return default;
    }

}
