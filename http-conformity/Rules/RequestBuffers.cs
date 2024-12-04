using HttpConformity.Infrastructure;
using HttpConformity.Model;

namespace HttpConformity.Rules;

public class RequestBuffers : IRule
{

    public HttpSpecification Specification => HttpSpecification.General;

    public string Requirement => "Server can receive requests in TCP fragments";

    public async Task<ValidationResult> ValidateAsync(Uri url)
    {
        await using var client = new AdvancedClient(url);

        await client.WriteAsync($"GET");
        await client.WriteAsync($" {client.Path} HTTP/1.0\r\nHost: {url.Host}\r\nConnection: close\r\n\r\n");

        var result = await client.ReadToEndAsync();

        if (result.Contains("200"))
        {
            return new ValidationResult(this, ValidationStatus.Passed);
        }

        return new ValidationResult(this, ValidationStatus.Failed, "Server did not respond with a 200 response");
    }

}
