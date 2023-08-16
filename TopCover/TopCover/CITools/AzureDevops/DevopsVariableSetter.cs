using TopCover.CoverageDiff;
using TopCover.Models;

namespace TopCover.CiTools.AzureDevops;

public class DevopsVariableSetter
{
    private readonly Action<string> _set;

    public DevopsVariableSetter(Action<string> set)
    {
        _set = set;
    }

    public void SetDevopsVars(CoverageDifference diff)
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
        
        var lineBreak = "%0D%0A";
        var comment = @$"```diff{lineBreak}{diff.FormatAsGitDiff().Replace(Environment.NewLine, lineBreak)}{lineBreak}```";
        SetVar("PR_COMMENT", comment);
    }
    
    private void SetVar(string name, string value) =>
        _set($"##vso[task.setvariable variable=TOPCOVER_{name}]{value}");
    
    private static string FormatChange(decimal diff) => $"{diff:+ ##0.0;- ##0.0;##0.0}";

}