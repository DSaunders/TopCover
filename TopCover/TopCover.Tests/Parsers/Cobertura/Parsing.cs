using FluentAssertions;
using TopCover.Parsers.Cobertura;
using TopCover.Tests.Helpers;

namespace TopCover.Tests.Parsers.Cobertura;

public class Parsing : IClassFixture<SampleLoader>
{
    private readonly SampleLoader _sampleLoader;
    private readonly CoverageReportGenerator _generator;

    public Parsing(SampleLoader sampleLoader)
    {
        _sampleLoader = sampleLoader;
        _generator = new CoverageReportGenerator();
    }

    [Fact]
    public async Task Throws_Exception_When_Stream_Cannot_Be_Parsed()
    {
        var sample = _sampleLoader.GetFile("Parsers.Cobertura._samples.bad.xml");
        var ex = await Assert.ThrowsAsync<CoberturaFileException>(async () => { await _generator.Generate(sample); });

        ex.Message.Should().Be("This file is not a Cobertura test report");
    }

    [Fact]
    public async Task Throws_Exception_When_Stream_Is_Not_Cobertura_File()
    {
        var sample = _sampleLoader.GetFile("Parsers.Cobertura._samples.incorrect.xml");
        var ex = await Assert.ThrowsAsync<CoberturaFileException>(async () => { await _generator.Generate(sample); });

        ex.Message.Should().Be("This file is not a Cobertura test report");
    }

    [Fact]
    public async Task Can_Parse_When_Doctype_Tag_Present()
    {
        var sample = _sampleLoader.GetFile("Parsers.Cobertura._samples.withdoctype.xml");
        await _generator.Generate(sample);
    }
}