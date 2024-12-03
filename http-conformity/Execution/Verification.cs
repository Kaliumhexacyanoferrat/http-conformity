using HttpConformity.Model;
using HttpConformity.Rules;

namespace HttpConformity.Execution;

public static class Verification
{

    public static async Task<ValidationSummary> RunAsync(string url, Action<ValidationResult> onResult)
    {
        bool warnings = false, failures = false;

        var uri = new Uri(url);

        foreach (var rule in RuleSet.All)
        {
            try
            {
                var result = await rule.ValidateAsync(uri);

                onResult(result);

                if (result.Status == ValidationStatus.Failed)
                {
                    failures = true;
                }
                else if (result.Status == ValidationStatus.Warning)
                {
                    warnings = true;
                }
            }
            catch (Exception e)
            {
                failures = true;
                onResult(new ValidationResult(rule, ValidationStatus.ExecutionFailed, null, e));
            }
        }

        if (failures) return ValidationSummary.Failed;

        if (warnings) return ValidationSummary.PassedWithWarnings;

        return ValidationSummary.Passed;
    }

}
