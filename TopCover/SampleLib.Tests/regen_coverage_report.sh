#!/bin/bash
dotnet test --settings coverlet.runsettings

cd ../TopCover.Tests/Parsers/Cobertura/_samples

cd */

mv  -v ./* ../

cd ..

find . -maxdepth 1 -mindepth 1 -type d -exec rm -rf '{}' \;

