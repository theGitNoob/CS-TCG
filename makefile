build:
	dotnet build

run:
	ENV=prod dotnet run --project WebServer
dev:
	ENV=dev dotnet watch run --project WebServer
