using System.Reflection;

namespace TopCover.Tests.Helpers;

/// <summary>
/// Loads embedded coverage reports
/// </summary>
public class SampleLoader
{
    private readonly Assembly _assembly;
    private readonly string? _assemblyName;

    public SampleLoader()
    {
        _assembly = Assembly.GetExecutingAssembly();
        _assemblyName = _assembly.GetName().Name;   
    }

    public Stream GetFile(string filename)
    {
        var result = _assembly.GetManifestResourceStream(
            $"{_assemblyName}.{filename}"
        );

        if (result is null)
            throw new Exception($"Sample {filename} could not be found");

        return result;
    }
}