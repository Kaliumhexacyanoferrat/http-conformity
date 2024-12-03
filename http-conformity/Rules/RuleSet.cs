using HttpConformity.Model;

namespace HttpConformity.Rules;

public class RuleSet
{
    private static readonly List<IRule> Rules = new()
    {
        new NotFound()
    };

    public static IEnumerable<IRule> All => Rules;

}
