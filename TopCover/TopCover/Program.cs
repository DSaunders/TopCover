using System.CommandLine;
using TopCover.ConsoleCommands;

var rootCommand = new RootCommand("TopCover");

rootCommand.AddCommand(DiffCommand.Get());

await rootCommand.InvokeAsync(args);

Console.ResetColor();
