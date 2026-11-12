.PHONY: link-agents run run-ns run-city-explorer test

link-agents:
	@./scripts/link-agents.sh

run:
	@dotnet run --project src/Duets.Cli/Duets.Cli.fsproj

run-ns:
	@dotnet run --project src/Duets.Cli/Duets.Cli.fsproj -- --no-saving

run-ce:
	@dotnet run --project src/Duets.CityExplorer/Duets.CityExplorer.fsproj

test:
	@dotnet test
