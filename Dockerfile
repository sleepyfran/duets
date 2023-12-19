FROM mcr.microsoft.com/dotnet/sdk:8.0

COPY *.sln .
COPY deps ./deps
COPY src ./src
COPY tests ./tests

RUN dotnet build
ENTRYPOINT ["dotnet", "run", "--project", "src/Cli/Cli.fsproj"]
