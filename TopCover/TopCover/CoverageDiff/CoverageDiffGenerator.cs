using TopCover.Models;

namespace TopCover.CoverageDiff;

public static class CoverageDiffGenerator
{
    public static CoverageDifference Diff(CoverageReport before, CoverageReport after)
    {
        var overallLineCoverage = new DiffSummary(
            before.Summary.LineCoverage,
            after.Summary.LineCoverage
        );

        var overallBranchCoverage = new DiffSummary(
            before.Summary.BranchCoverage,
            after.Summary.BranchCoverage
        );

        var overallLineCount = new DiffSummary(
            before.Summary.TotalLines,
            after.Summary.TotalLines
        );
        
        var coveredLinesCount = new DiffSummary(
            before.Summary.CoveredLines,
            after.Summary.CoveredLines
        );

        return new CoverageDifference(
            new CoverageDifferenceSummary(
                overallLineCoverage,
                overallBranchCoverage,
                overallLineCount,
                coveredLinesCount
            )
        );
    }
}