clear

dotnet run --project ./TopCover/TopCover/TopCover.csproj -- diff --before ./CoverageSamples/old.xml --after ./CoverageSamples/new.xml

dotnet run --project ./TopCover/TopCover/TopCover.csproj -- diff -b ./CoverageSamples/new.ml -a ./CoverageSamples/old.xml --setvars devops
