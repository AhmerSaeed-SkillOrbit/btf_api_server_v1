FROM microsoft/dotnet:2.2-sdk
WORKDIR /app

# copy csproj and restore as distinct layers
COPY BurmaTaskForce/BurmaTaskForce/Btf.Web.Api.csproj ./
RUN dotnet restore

# copy and build everything else
COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "bin/Debug/netcoreapp2.2/Btf.Web.Api.dll"]

