using System.Net;
using HttpConformity.Model;

namespace HttpConformity.Rules;

public class NotFound : IRule
{

    public HttpSpecification Specification => HttpSpecification.RFC9110;

    public string Section => "15.5.5";

    public string Requirement => "Server should return 404 Not Found for unknown resources";

    public async Task<ValidationResult> ValidateAsync(Uri url)
    {
        var builder = new UriBuilder(url);

        builder.Path = "idomostcertainlynotexist";

        using var client = Infrastructure.CreateClient();

        using var result = await client.GetAsync(builder.Uri);

        if (result.StatusCode != HttpStatusCode.NotFound)
        {
            return new ValidationResult(this, ValidationStatus.Warning, "Server should return 404 for unknown resources");
        }

        return new ValidationResult(this, ValidationStatus.Passed);
    }

}
