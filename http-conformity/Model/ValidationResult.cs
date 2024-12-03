namespace HttpConformity.Model;

public record ValidationResult(IRule Rule, ValidationStatus Status, string? Reason = null, Exception? Failure = null);
