# TopCover

![nuget badge](https://img.shields.io/nuget/vpre/TopCover)


> If a part of your test suite is weak in a way that coverage can detect, it's likely also weak in a way coverage can't detect.
>
> -- [Brian Marick](http://www.exampler.com/testing-com/writings/coverage.pdf)

Chasing abirtrary test coverage numbers is bad, but having no test coverage is _also_ bad!

TopCover is a command-line tool, designed to run in a CI process, that helps you analyse test coverage and understand your code's weak points.

TopCover does not run code coverage tools. Instead, you pass it code coverage reports from your tool of choice.

Design Goals:
- All options must available from the command line, JSON files in every repository is not necessary


Features yet to be implemented:
- Find tests that change a lot when an area of code is refactored. This could mean that the tests are tightly coupled to the implementation
- Specify coverage thresholds for key areas of your code (e.g. database code must be tested, but controller don't)
- Check code coverage on new code in a Pull Request (and, optionally, set a threshold)

Current only the Cobertura format is supported. More formats will follow.

# Diff Code Coverage

By providing two coverage reports (e.g. from a PR and the target branch it will be merged in to), TopCover can analyse the difference.

```
topcover diff -?
```

## Arguments

### Before and After (required)

The relative path to the Cobertura XML test reports to compare.

For example, you might collect test coverage from a Pull Request (_'after'_) and the `main` branch (_'before'_), to understand how the pull request changes the amount of test coverage.

```
topcover diff --before old.xml --after new.xml
```

(alias: `-a` and `-b`)
```
topcover diff -b old.xml -a new.xml
```

produces:
```
Coverage Change Report
----------------------

  Summary:
    Line coverage     88.9% -> 55.6%  -33.3%
    Branch coverage   50.0% -> 50.0%  (no change)
```

### Saving results to pipline variables

Stores the results of the diff in pipeline variables, to be accessed in later build steps.

This currently supports [Azure Devops](https://learn.microsoft.com/en-us/azure/devops/pipelines/process/variables?view=azure-devops&tabs=classic%2Cbatch#set-a-job-scoped-variable-from-a-script-1), but will soon support [GitHub Actions](https://docs.github.com/en/actions/using-workflows/workflow-commands-for-github-actions) too.


```
topcover diff -b old.xml -a new.xml --setvars devops
```

sets the following variables:
```
TOPCOVER_OVERALL_LINE_BEFORE = 88.8
TOPCOVER_OVERALL_LINE_AFTER = 55.5
TOPCOVER_OVERALL_LINE_CHANGE = -33.3
TOPCOVER_OVERALL_LINE_CHANGE_INDICATOR = ↓
TOPCOVER_OVERALL_BRANCH_BEFORE = 50.0
TOPCOVER_OVERALL_BRANCH_AFTER = 50.0
TOPCOVER_OVERALL_BRANCH_CHANGE = 0.0
TOPCOVER_OVERALL_BRANCH_CHANGE_INDICATOR = 
```