using HttpConformity.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<TestCommand>("test");
});

return await app.RunAsync(args);
