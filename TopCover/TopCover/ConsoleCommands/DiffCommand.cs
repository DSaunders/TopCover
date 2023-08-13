using System.CommandLine;
using System.Drawing;
using TopCover.CoverageDiff;
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

        var storeInVars = new Option<string?>(
            name: "--setvars",
            description: "Stores the results of the diff in pipeline variables (options: \"devops\")"
        );

        var command = new Command("diff", "Calculate the difference between two coverage reports")
        {
            diffBeforeOption,
            diffAfterOption,
            storeInVars
        };

        command.SetHandler(async (before, after, env) =>
            {
                if (!before.Exists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        $"Could not find the 'before' file '{before.FullName}'.");
                    Console.ResetColor();
                    return;
                }

                if (!after.Exists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        $"Could not find the 'after' file '{after.FullName}'.");
                    Console.ResetColor();
                    return;
                }

                await using Stream oldFile = before.OpenRead();
                await using Stream newFile = after.OpenRead();

                var generator = new CoverageReportGenerator();

                var oldReport = await generator.Generate(oldFile);
                var newReport = await generator.Generate(newFile);

                var diff = CoverageDiffGenerator.Diff(oldReport, newReport);
                WriteReport(diff);

                if (env is not null && env == "devops")
                    StoreResultsInVars(diff);
            },
            diffBeforeOption,
            diffAfterOption,
            storeInVars
        );

        return command;
    }

    private static void WriteReport(CoverageDifference diff)
    {
        Console.WriteLine();
        Console.WriteLine("Coverage Change Report");
        Console.WriteLine("----------------------");
        Console.WriteLine();
        Console.WriteLine("  Summary:");
        OutputColouredMetric("Line coverage   ", diff.Summary.LineCoverage);
        OutputColouredMetric("Branch coverage ", diff.Summary.BranchCoverage);
        Console.WriteLine();
    }

    private static void OutputColouredMetric(string name, DiffSummary diff)
    {
        Console.Write($"    {name}  ");
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.Write($"{diff.Old:#00.0}% -> {diff.New:#00.0}%  ");

        if (diff.Change == 0)
        {
            Console.Write($"(no change)");
            Console.WriteLine();
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = diff.Change < 0 ? ConsoleColor.Red : ConsoleColor.Green;

        Console.Write($"{FormatChange(diff.Change)}%");
        Console.WriteLine();

        Console.ResetColor();
    }

    private static void StoreResultsInVars(CoverageDifference diff)
    {
        SetVar("OVERALL_LINE_BEFORE", diff.Summary.LineCoverage.Old.ToString("#00.0"));
        SetVar("OVERALL_LINE_AFTER", diff.Summary.LineCoverage.New.ToString("#00.0"));
        SetVar("OVERALL_LINE_CHANGE", FormatChange(diff.Summary.LineCoverage.Change));
        var lineChangeChar = diff.Summary.LineCoverage.Change > 0
            ? "↑"
            : diff.Summary.LineCoverage.Change < 0
                ? "↓"
                : string.Empty;
        SetVar("OVERALL_LINE_CHANGE_INDICATOR", lineChangeChar);
        Console.WriteLine();
        SetVar("OVERALL_BRANCH_BEFORE", diff.Summary.BranchCoverage.Old.ToString("#00.0"));
        SetVar("OVERALL_BRANCH_AFTER", diff.Summary.BranchCoverage.New.ToString("#0.0"));
        SetVar("OVERALL_BRANCH_CHANGE", FormatChange(diff.Summary.BranchCoverage.Change));
        var branchChangeChar = diff.Summary.BranchCoverage.Change > 0
            ? "↑"
            : diff.Summary.BranchCoverage.Change < 0
                ? "↓"
                : string.Empty;
        SetVar("OVERALL_BRANCH_CHANGE_INDICATOR", branchChangeChar);
    }

    private static string FormatChange(decimal diff) => $"{diff:+##0.0;-##0.0;##0.0}";

    private static void SetVar(string name, string value) =>
        Console.WriteLine(
            $"##vso[task.setvariable variable=TOPCOVER_{name}]{value}");
}