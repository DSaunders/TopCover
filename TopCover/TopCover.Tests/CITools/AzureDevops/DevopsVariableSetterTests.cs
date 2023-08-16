using FluentAssertions;
using FluentAssertions.Execution;
using TopCover.CiTools.AzureDevops;
using TopCover.Models;

namespace TopCover.Tests.CITools.AzureDevops;

public class DevopsVariableSetterTests
{
    [Fact]
    public void Sets_Devops_Variables_When_Values_Change()
    {
        var diff = GenerateCoverageDiff(10m, 20m, 40m, 30m);
        
        var commands = new List<string>();
        var setter = new DevopsVariableSetter(commands.Add);
        
        setter.SetDevopsVars(diff);

        using var _ = new AssertionScope();
        
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_LINE_BEFORE", "10.0");
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_LINE_AFTER", "20.0");
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_LINE_CHANGE", "+ 10.0");
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_LINE_CHANGE_INDICATOR", "↑");
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_BRANCH_BEFORE", "40.0");
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_BRANCH_AFTER", "30.0");
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_BRANCH_CHANGE", "- 10.0");
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_BRANCH_CHANGE_INDICATOR", "↓");
    }

    [Fact]
    public void Sets_Change_Variables_Correctly_When_No_Change_In_Coverage()
    {
        var diff = GenerateCoverageDiff(00m, 00m, 20m, 20m);

        var commands = new List<string>();
        var setter = new DevopsVariableSetter(commands.Add);

        setter.SetDevopsVars(diff);
        
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_LINE_CHANGE", "0.0");
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_LINE_CHANGE_INDICATOR", "");
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_BRANCH_CHANGE", "0.0");
        VariableShouldBeSet(commands, "TOPCOVER_OVERALL_BRANCH_CHANGE_INDICATOR", "");
    }

    [Fact]
    public void Sets_Devops_PR_Comment_In_Markdown()
    {
        var diff = GenerateCoverageDiff(00m, 00m, 20m, 20m);

        var commands = new List<string>();
        var setter = new DevopsVariableSetter(commands.Add);

        setter.SetDevopsVars(diff);

        // %0D%0A is a line break in DevOps YAML
        var expectedComment =
            "```diff%0D%0A" +
            $"{diff.FormatAsGitDiff().Replace(Environment.NewLine, "%0D%0A")}%0D%0A" +
            "```";
        VariableShouldBeSet(commands, "TOPCOVER_PR_COMMENT", expectedComment);
    }

    private void VariableShouldBeSet(List<string> commands, string variable, string value)
    {
        var correctCommand = $"##vso[task.setvariable variable={variable}]{value}";
        commands.Should().Contain(correctCommand);
    }

    private CoverageDifference GenerateCoverageDiff(decimal lineOld, decimal lineNew, decimal branchOld,
        decimal branchNew) =>
        new(new CoverageDifferenceSummary(
            new DiffSummary(lineOld, lineNew),
            new DiffSummary(branchOld, branchNew)
        ));
}