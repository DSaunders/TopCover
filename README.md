# TopCover

![nuget badge](https://img.shields.io/nuget/vpre/TopCover)

Chasing 100% code coverage is bad, but having no test coverage is _also_ bad!

> If a part of your test suite is weak in a way that coverage can detect, it's likely also weak in a way coverage can't detect.
>
> -- [Brian Marick](http://www.exampler.com/testing-com/writings/coverage.pdf)

<br />
TopCover is a command-line tool, designed to run in a CI process, that helps you analyse test coverage and understand your code's weak points.

TopCover does not run code coverage tools. Instead, you pass it code coverage reports from your tool of choice.

<br />

# Installation

Install with `dotnet tool`:

```bash
dotnet tool install --global TopCover
```

# Features

## # Diff Code Coverage Reports

By providing two coverage reports (e.g. from a PR and the target branch it will be merged in to), TopCover can analyse the difference.

```
topcover diff -?
```

### `--before` and `--after` (required)

The relative path to the Cobertura XML test reports to compare.

For example, you might collect test coverage from a Pull Request (_'after'_) and the `main` branch (_'before'_), to understand how the pull request changes the amount of test coverage.

```
topcover diff --before old.xml --after new.xml
```

or use the aliases `-a` and `-b`

```
topcover diff -b old.xml -a new.xml
```

produces:
```
 |===============================================================|
 |                   |   Before   |    After   |    Change +/-   |
 |===============================================================|
+|  Line Coverage    |   1.0%     |    2.0%    |    + 1.0%       |
-|  Branch Coverage  |   9.9%     |    9.5%    |    - 0.4%       |
 |===============================================================|
```

### `--setvars`

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

# Coming soon

 - Find tests that change a lot when an area of code is refactored. This could mean that the tests are tightly coupled to the implementation
 - Specify coverage thresholds for key areas of your code (e.g. database code must be tested, but controller don't)
 - Check code coverage on new code in a Pull Request (and, optionally, set a threshold)
