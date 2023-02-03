build:
	dotnet build

run:
	dotnet run enviroment=development --project WebServer
dev:
	dotnet watch run --project WebServer
