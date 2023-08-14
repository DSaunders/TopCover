using System.Text;

namespace TopCover;

public static class DiffDisplay
{
    public static string FormatAsGitDiff(this CoverageDifference diff)
    {
        var s = new StringBuilder();

        // Generate header lines
        s.AppendLine(" |===============================================================|");
        s.AppendLine(" |                   |   Before   |    After   |    Change +/-   |");
        s.AppendLine(" |===============================================================|");
        s.AppendLine("{lcs}|  Line Coverage    |   {lca}    |    {lcb}   |    {lcc}      |");
        s.AppendLine("{bcs}|  Branch Coverage  |   {bca}    |    {bcb}   |    {bcc}      |");
        s.AppendLine(" |===============================================================|");

        var template = s.ToString();

        return template
            .Replace("{lca}", FormatVal(diff.Summary.LineCoverage.Old))
            .Replace("{lcb}", FormatVal(diff.Summary.LineCoverage.New))
            .Replace("{lcc}", FormatChange(diff.Summary.LineCoverage.Change))
            .Replace("{lcs}", GetLineSymbol(diff.Summary.LineCoverage.Change))
            .Replace("{bca}", FormatVal(diff.Summary.BranchCoverage.Old))
            .Replace("{bcb}", FormatVal(diff.Summary.BranchCoverage.New))
            .Replace("{bcc}", FormatChange(diff.Summary.BranchCoverage.Change))
            .Replace("{bcs}", GetLineSymbol(diff.Summary.BranchCoverage.Change));
    }

    private static string FormatChange(decimal diff) => $"{diff:+ ##0.0;- ##0.0;##0.0}%".PadRight(7);
    private static string FormatVal(decimal diff) => $"{diff:##0.0}%".PadRight(5);
    private static string GetLineSymbol(decimal diff) => diff > 0 ? "+" : diff < 0 ? "-" : " ";
} 