namespace HttpConformity.Model;

public interface IRule
{

    HttpSpecification Specification { get; }

    string? Section => null;

    string Requirement { get; }

    Task<ValidationResult> ValidateAsync(Uri url);

}
