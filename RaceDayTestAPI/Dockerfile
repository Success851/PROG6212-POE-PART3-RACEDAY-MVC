
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RaceDayTestAPI.csproj", "./"]
RUN dotnet restore "RaceDayTestAPI.csproj"

COPY . .
RUN dotnet publish "RaceDayTestAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RaceDayTestAPI.dll"]
