using FluentAssertions;
using FluentAssertions.Execution;
using TopCover.Parsers.Cobertura;
using TopCover.Tests.Helpers;

namespace TopCover.Tests.Parsers.Cobertura;

public class Structure : IClassFixture<SampleLoader>
{
    private readonly CoverageReportGenerator _generator;
    private readonly Stream _sample;

    public Structure(SampleLoader sampleLoader)
    {
        _generator = new CoverageReportGenerator();

        _sample = sampleLoader.GetFile(
            "Parsers.Cobertura._samples.summary.xml"
        );
    }
    
    [Fact]
    public async Task Returns_Correct_Package_Class_Method_Structure()
    {
        var result = await _generator.Generate(_sample);

        using var _ = new AssertionScope();
        
        result.Packages.Count.Should().Be(1);
        result.Packages[0].Name.Should().Be("SampleLib");
        
        result.Packages[0].Classes.Count.Should().Be(1);
        result.Packages[0].Classes[0].Name.Should().Be("SampleClass.HelloClass");

        var sampleClass = result.Packages[0].Classes[0];
        sampleClass.Methods.Count.Should().Be(2);
        sampleClass.Methods[0].Name.Should().Be("Hello");
        sampleClass.Methods[1].Name.Should().Be("Untested");
    }
    
    [Fact]
    public async Task Reports_Coverage_For_Classes()
    {
        var result = await _generator.Generate(_sample);

        using var _ = new AssertionScope();

        result.Packages[0].Classes[0].LineCoverage.Should().Be(55.55m);
        result.Packages[0].Classes[0].BranchCoverage.Should().Be(50m);
    }
}