using HttpConformity.Model;

namespace HttpConformity.Rules;

public static class RuleSet
{
    private static readonly List<IRule> Rules = new()
    {

        // general TCP handling
        new RequestBuffers(),

        // HTTP server protocol
        new NotFound()

    };

    public static IEnumerable<IRule> All => Rules;

}
