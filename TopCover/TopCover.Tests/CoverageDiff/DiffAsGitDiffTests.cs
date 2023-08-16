using FluentAssertions;
using TopCover.Models;

namespace TopCover.Tests.CoverageDiff;

public class DiffAsGitDiffTests
{
    [Fact]
    public void Display_Single_Digit_Coverage_Correctly()
    {
        var cov = GenerateCoverageDiff(1.0m, 2.0m, 9.9m, 9.5m);

        var target = @"
 |===============================================================|
 |                   |   Before   |    After   |    Change +/-   |
 |===============================================================|
+|  Line Coverage    |   1.0%     |    2.0%    |    + 1.0%       |
-|  Branch Coverage  |   9.9%     |    9.5%    |    - 0.4%       |
 |===============================================================|
";

        cov.FormatAsGitDiff().Should().Be(target.Substring(2));
    }

    [Fact]

    public void Display_Double_Digit_Coverage_Correctly_To_1dp()
    {
        var cov = GenerateCoverageDiff(13.54m, 23.54m, 9.9m, 9.5m);

        var target = @"
 |===============================================================|
 |                   |   Before   |    After   |    Change +/-   |
 |===============================================================|
+|  Line Coverage    |   13.5%    |    23.5%   |    + 10.0%      |
-|  Branch Coverage  |   9.9%     |    9.5%    |    - 0.4%       |
 |===============================================================|
";

        cov.FormatAsGitDiff().Should().Be(target.Substring(2));
    }
    
    private CoverageDifference GenerateCoverageDiff(decimal lineOld, decimal lineNew, decimal branchOld,
        decimal branchNew) =>
        new(new CoverageDifferenceSummary(
            new DiffSummary(lineOld, lineNew),
            new DiffSummary(branchOld, branchNew)
        ));
}