clear

dotnet run --project ./TopCover/TopCover/TopCover.csproj -- diff -b ./CoverageSamples/old.xml -a ./CoverageSamples/new.xml --setDevopsVars
