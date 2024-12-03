using System.ComponentModel;
using System.Net;
using HttpConformity.Execution;
using HttpConformity.Model;
using Spectre.Console;
using Spectre.Console.Cli;

namespace HttpConformity.Commands;

public sealed class TestCommand : AsyncCommand<TestCommand.Settings>
{
    public const int SUCCESS = 1;
    public const int NOT_AVAILABLE = 2;
    public const int FAILED = 3;

    public sealed class Settings : CommandSettings
    {

        [Description("The URL to test. Should return a 200 status code for a simple GET.")]
        [CommandArgument(0, "[url]")]
        public required string Server { get; init; }

    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (!await IsAvailable(settings.Server))
        {
            return NOT_AVAILABLE;
        }

        int passed = 0, warnings = 0, failed = 0, total = 0;

        await Verification.RunAsync(settings.Server, (result) =>
        {
            var color = result.Status switch
            {
                ValidationStatus.Failed => "red",
                ValidationStatus.Warning => "yellow",
                ValidationStatus.Passed => "green",
                ValidationStatus.ExecutionFailed => "red"
            };

            AnsiConsole.Markup($"[{color}]({result.Status.ToString().ToUpper()})[/] {result.Rule.Specification} {result.Rule.Section}: {result.Rule.Requirement}");

            if (result.Reason != null)
            {
                Console.WriteLine($"  {result.Reason}");
            }

            if (result.Failure != null)
            {
                AnsiConsole.WriteException(result.Failure, ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes | ExceptionFormats.ShortenMethods | ExceptionFormats.ShowLinks);
            }

            total++;

            if (result.Status == ValidationStatus.Passed)
            {
                passed++;
            }
            else if (result.Status == ValidationStatus.Warning)
            {
                passed++;
                warnings++;
            }
            else
            {
                failed++;
            }
        });

        Console.WriteLine();

        var table = new Table();

        table.AddColumn("Category");
        table.AddColumn("Count");

        table.AddRow("Total", total.ToString());
        table.AddRow("Passed", passed.ToString());
        table.AddRow("Warnings",warnings.ToString());
        table.AddRow("Failed", passed.ToString());

        AnsiConsole.Write(table);

        return (failed == 0) ? SUCCESS : FAILED;
    }

    private static async Task<bool> IsAvailable(string url)
    {
        try
        {
            using var client = Infrastructure.CreateClient();

            using var result = await client.GetAsync(url);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                AnsiConsole.Markup($"[red]Server returned with status code {result.StatusCode}[/]");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes | ExceptionFormats.ShortenMethods | ExceptionFormats.ShowLinks);
            return false;
        }
    }

}
