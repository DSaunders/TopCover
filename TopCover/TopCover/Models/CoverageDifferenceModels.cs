namespace TopCover.Models;

public record CoverageDifference(CoverageDifferenceSummary Summary);

public record CoverageDifferenceSummary(
    DiffSummary LineCoverage, 
    DiffSummary BranchCoverage,
    DiffSummary TotalLines,
    DiffSummary CoveredLines
);

public record DiffSummary(decimal Old, decimal New)
{
    public decimal Change => New - Old;
}