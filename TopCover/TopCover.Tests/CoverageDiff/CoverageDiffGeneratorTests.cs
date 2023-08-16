using FluentAssertions;
using TopCover.CoverageDiff;
using TopCover.Models;

namespace TopCover.Tests.CoverageDiff;

public class CoverageDiffGeneratorTests
{
    [Fact]
    public void Calculates_Overall_Line_Coverage_Change()
    {
        var oldReport = new CoverageReport(
            new Summary(45.40m, 0, 0, 0), 
            new List<Package>()
        );
        var newReport = new CoverageReport(
            new Summary(55.30m, 0, 0, 0), 
            new List<Package>()
        );

        var diff = CoverageDiffGenerator.Diff(oldReport, newReport);

        diff.Summary.LineCoverage.Old.Should().Be(45.40m);
        diff.Summary.LineCoverage.New.Should().Be(55.30m);
        diff.Summary.LineCoverage.Change.Should().Be(+9.90m);
    }
    
    [Fact]
    public void Calculates_Overall_Branch_Coverage_Change()
    {
        var oldReport = new CoverageReport(
            new Summary(0, 45.40m, 0, 0), 
            new List<Package>()
        );
        var newReport = new CoverageReport(
            new Summary(0, 55.30m, 0, 0), 
            new List<Package>()
        );

        var diff = CoverageDiffGenerator.Diff(oldReport, newReport);

        diff.Summary.BranchCoverage.Old.Should().Be(45.40m);
        diff.Summary.BranchCoverage.New.Should().Be(55.30m);
        diff.Summary.BranchCoverage.Change.Should().Be(+9.90m);
    }
    
    [Fact]
    public void Calculates_Line_Count_Change()
    {
        var oldReport = new CoverageReport(
            new Summary(0, 0, 3, 0), 
            new List<Package>()
        );
        var newReport = new CoverageReport(
            new Summary(0, 0, 5, 0), 
            new List<Package>()
        );

        var diff = CoverageDiffGenerator.Diff(oldReport, newReport);

        diff.Summary.TotalLines.Old.Should().Be(3);
        diff.Summary.TotalLines.New.Should().Be(5);
        diff.Summary.TotalLines.Change.Should().Be(+2);
    }
    
    [Fact]
    public void Calculates_Lines_Covered_Change()
    {
        var oldReport = new CoverageReport(
            new Summary(0, 0, 0, 3), 
            new List<Package>()
        );
        var newReport = new CoverageReport(
            new Summary(0, 0, 0, 5), 
            new List<Package>()
        );

        var diff = CoverageDiffGenerator.Diff(oldReport, newReport);

        diff.Summary.CoveredLines.Old.Should().Be(3);
        diff.Summary.CoveredLines.New.Should().Be(5);
        diff.Summary.CoveredLines.Change.Should().Be(+2);
    }
}