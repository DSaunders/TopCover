using System.CommandLine;
using TopCover.CiTools.AzureDevops;
using TopCover.CoverageDiff;
using TopCover.Models;
using TopCover.Parsers.Cobertura;

namespace TopCover.ConsoleCommands;

public static class DiffCommand
{
    public static Command Get()
    {
        var diffBeforeOption = new Option<FileInfo>(
            name: "--before",
            description: "The coverage report for the 'before' state of the code"
        );
        diffBeforeOption.AddAlias("-b");

        var diffAfterOption = new Option<FileInfo>(
            name: "--after",
            description: "The coverage report for the 'after' state of the code"
        );
        diffAfterOption.AddAlias("-a");

        var storeInVars = new Option<bool>(
            name: "--setDevopsVars",
            description: "Stores the results of the diff in Azure DevOps pipeline variables"
        );

        var newLineChar = new Option<string?>(
            name: "--newlineChar"
        );
        
        var command = new Command("diff", "Calculate the difference between two coverage reports")
        {
            diffBeforeOption,
            diffAfterOption,
            storeInVars,
            newLineChar
        };

        command.SetHandler(async (
                before, 
                after, 
                devopsVars
                ) =>
            {
                if (!before.Exists)
                {
                    WriteError($"Could not find the 'before' file '{before.FullName}'.");
                    Environment.Exit(1);
                }

                if (!after.Exists)
                {
                    WriteError($"Could not find the 'after' file '{after.FullName}'.");
                    Environment.Exit(1);
                }

                await using Stream oldFile = before.OpenRead();
                await using Stream newFile = after.OpenRead();

                var generator = new CoverageReportGenerator();

                var oldReport = await generator.Generate(oldFile);
                var newReport = await generator.Generate(newFile);

                var diff = CoverageDiffGenerator.Diff(oldReport, newReport);
                WriteReport(diff);

                if (devopsVars)
                {
                    new DevopsVariableSetter(Console.WriteLine)
                        .SetDevopsVars(diff);
                }
            },
            diffBeforeOption,
            diffAfterOption,
            storeInVars
            );

        return command;
    }

    private static void WriteError(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);
        Console.ResetColor();
    }

    private static void WriteReport(CoverageDifference diff)
    {
        Console.WriteLine(diff.FormatAsGitDiff());
    }
}