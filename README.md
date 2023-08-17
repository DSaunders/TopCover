# TopCover


TopCover is a command-line tool, designed to run in a CI process, that helps you analyse test coverage and understand your code's weak points.

TopCover does not run code coverage tools. Instead, you pass it code coverage reports from your tool of choice.

# Installation

Install with `dotnet tool`:

```bash
dotnet tool install --global TopCover
```

# Features

## # Diff Code Coverage Reports

By providing two coverage reports (e.g. from a PR and the target branch it will be merged in to), TopCover can analyse the difference.

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

### `--setDevopsVars`

Stores the results of the diff in [Azure Devops](https://learn.microsoft.com/en-us/azure/devops/pipelines/process/variables?view=azure-devops&tabs=classic%2Cbatch#set-a-job-scoped-variable-from-a-script-1) variables, to be accessed in later build steps.

```
topcover diff -b old.xml -a new.xml --setDevopsVars
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

This also stores the full coverage report in a variable; `TOPCOVER_PR_COMMENT`.

This comment is wrapped in a markdown ` ```diff ` code tag, so that it is displayed nicely in a PR comment, e.g. in GitHub:

```yaml
- task: GitHubComment@0
  inputs:
    gitHubConnection: "MyGitHubConnection"
    repositoryName: "$(Build.Repository.Name)"
    comment: "$(TOPCOVER_PR_COMMENT)"
```

![image](https://github.com/DSaunders/TopCover/assets/4059030/3bb08947-8401-417b-9aa9-c18ba39ef129)



# Coming soon

 - Find tests that change a lot when an area of code is refactored. This could mean that the tests are tightly coupled to the implementation
 - Specify coverage thresholds for key areas of your code (e.g. database code must be tested, but controller don't)
 - Check code coverage on new code in a Pull Request (and, optionally, set a threshold)
