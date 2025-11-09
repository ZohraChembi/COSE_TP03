FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CostumerService.sln ./
COPY CostumerService.Api/CostumerService.Api.csproj CostumerService.Api/

RUN dotnet restore CostumerService.sln

COPY . .

RUN dotnet publish CostumerService.Api/CostumerService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CostumerService.Api.dll"]
