build:
	dotnet build

run:
	dotnet run --environment Production --project WebServer
dev:
	dotnet watch run --project WebServer
