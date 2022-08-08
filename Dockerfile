FROM mcr.microsoft.com/dotnet/sdk:6.0

COPY *.sln .
COPY src ./src
COPY tests ./tests

RUN dotnet build
ENTRYPOINT ["dotnet", "run", "--project", "src/Cli/Cli.fsproj"]