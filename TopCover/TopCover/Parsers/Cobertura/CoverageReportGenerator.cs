using System.Xml;
using System.Xml.Serialization;
using TopCover.Parsers.Cobertura.XmlModels;

namespace TopCover.Parsers.Cobertura;

public class CoverageReportGenerator
{
    public Task<CoverageReport> Generate(Stream report)
    {
        var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };

        using var xmlReader = XmlReader.Create(report, settings);
            
        var serializer = new XmlSerializer(typeof(Coverage));

        Coverage? results;
        try
        {
            results = (Coverage)serializer.Deserialize(xmlReader)!;
        }
        catch (InvalidOperationException)
        {
            throw new CoberturaFileException("This file is not a Cobertura test report");
        }

        var summary = new Summary(
            results.LineRate * 100,
            results.BranchRate * 100
        );

        return Task.FromResult(
            new CoverageReport(
                summary,
                GetPackages(results)
            )
        );
    }

    private List<Package> GetPackages(Coverage coverage) =>
        coverage.Packages.Items
            .Select(p => new Package(p.Name, GetClasses(p)))
            .ToList();

    private List<Class> GetClasses(XmlModels.Package package) =>
        package.Classes.Items
            .Select(c => new Class(
                c.Name,
                c.LineRate * 100,
                c.BranchRate * 100,
                GetMethods(c)
            ))
            .ToList();

    private List<Method> GetMethods(XmlModels.Class @class) =>
        @class.Methods.Items.Select(m => new Method(m.Name)).ToList();
}