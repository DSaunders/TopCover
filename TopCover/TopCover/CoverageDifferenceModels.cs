namespace TopCover;

public record CoverageDifference(CoverageDifferenceSummary Summary);

public record CoverageDifferenceSummary(DiffSummary LineCoverage, DiffSummary BranchCoverage);

public record DiffSummary(decimal Old, decimal New)
{
    public decimal Change => New - Old;
}