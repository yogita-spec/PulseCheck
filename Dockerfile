
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/PulseCheck.Api/*.csproj ./src/PulseCheck.Api/
RUN dotnet restore src/PulseCheck.Api/PulseCheck.Api.csproj

COPY src/PulseCheck.Api/ ./src/PulseCheck.Api/
RUN dotnet publish src/PulseCheck.Api/PulseCheck.Api.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PulseCheck.Api.dll"]
