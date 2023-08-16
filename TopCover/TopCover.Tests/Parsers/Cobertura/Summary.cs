using FluentAssertions;
using TopCover.Parsers.Cobertura;
using TopCover.Tests.Helpers;

namespace TopCover.Tests.Parsers.Cobertura;

public class SummaryTests : IClassFixture<SampleLoader>
{
    private readonly CoverageReportGenerator _generator;
    private readonly Stream _sample;

    public SummaryTests(SampleLoader sampleLoader)
    {
        _generator = new CoverageReportGenerator();

        _sample = sampleLoader.GetFile(
            "Parsers.Cobertura._samples.summary.xml"
        );
    }
    
    [Fact]
    public async Task Returns_Overall_Sequence_Coverage_Rounded_To_2dp()
    {
        (await _generator.Generate(_sample))
            .Summary
            .LineCoverage
            .Should().Be(55.55m);
    }
    
    [Fact]
    public async Task Returns_Overall_Branch_Coverage_Rounded_To_2dp()
    {
        (await _generator.Generate(_sample))
            .Summary
            .BranchCoverage
            .Should().Be(50.00m);
    }
    
    [Fact]
    public async Task Returns_Total_Lines()
    {
        (await _generator.Generate(_sample))
            .Summary
            .TotalLines
            .Should().Be(9);
    }
    
    [Fact]
    public async Task Returns_Covered_Lines()
    {
        (await _generator.Generate(_sample))
            .Summary
            .CoveredLines
            .Should().Be(5);
    }
}