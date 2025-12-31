# add services
``` bash
cd src/Modules
dotnet new classlib -n {name}.Service
cd ..
dotnet sln ../trpg-mcp.sln add ./Modules/{name}.Service/{name}.Service.csproj
dotnet add ./ToolBox/ToolBox.csproj reference ./Modules/{name}.Service/{name}.Service.csproj
```