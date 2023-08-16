namespace TopCover.Models;

public record CoverageReport(
    Summary Summary,
    List<Package> Packages
);

public record Summary(
    decimal LineCoverage,
    decimal BranchCoverage,
    decimal TotalLines,
    decimal CoveredLines
);

public record Package(string Name, List<Class> Classes);

public record Class(
    string Name, 
    decimal LineCoverage, 
    decimal BranchCoverage, 
    List<Method> Methods
);

public record Method(string Name);