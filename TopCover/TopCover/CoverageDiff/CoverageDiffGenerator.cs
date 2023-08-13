namespace TopCover.CoverageDiff;

public static class CoverageDiffGenerator
{
    public static CoverageDifference Diff(CoverageReport before, CoverageReport after)
    {
        var overallLineCoverage = new DiffSummary(
            before.Summary.LineCoverage,
            after.Summary.LineCoverage,
            after.Summary.LineCoverage - before.Summary.LineCoverage
        );

        var overallBranchCoverage = new DiffSummary(
            before.Summary.BranchCoverage,
            after.Summary.BranchCoverage,
            after.Summary.BranchCoverage - before.Summary.BranchCoverage
        );

        return new CoverageDifference(
            new CoverageDifferenceSummary(
                overallLineCoverage,
                overallBranchCoverage
            )
        );
    }
}