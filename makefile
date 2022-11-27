build:
	dotnet build

run:
	dotnet run --project WebServer
dev:
	dotnet watch run --project WebServer
test:
	dotnet run --project Tester
