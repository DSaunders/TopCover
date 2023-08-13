namespace TopCover;

public record CoverageReport(
    Summary Summary,
    List<Package> Packages
);

public record Summary(
    decimal LineCoverage,
    decimal BranchCoverage
);

public record Package(string Name, List<Class> Classes);

public record Class(
    string Name, 
    decimal LineCoverage, 
    decimal BranchCoverage, 
    List<Method> Methods
);

public record Method(string Name);