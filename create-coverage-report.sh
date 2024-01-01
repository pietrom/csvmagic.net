#!/bin/bash
SOLUTION_NAME="CsvMagic"
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover  /p:Include="[$SOLUTION_NAME*]*" /p:Exclude="[xunit.*]*" /p:Exclude="[nunit.*]*"
if [ ! -f ./tools/reportgenerator ] ; then
	dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools
fi
./tools/reportgenerator -reports:**/*coverage*.xml -targetdir:./html-report "-assemblyfilters:+${SOLUTION_NAME};+${SOLUTION_NAME}.*"
