..\..\.nuget\nuget pack VortexTEliteProtocol.csproj
for %%x in (*.nupkg) do ..\..\.nuget\nuget push "%%x" -s http://nuget.swisstxt.ch/ Nug37@5w1557x7
del *.nupkg