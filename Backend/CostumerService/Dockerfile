# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and csproj first for caching
COPY CostumerService.sln ./
COPY CostumerService.Api/CostumerService.Api.csproj CostumerService.Api/

# Restore only
RUN dotnet restore CostumerService.sln

# Copy the rest of the source code
COPY . .

# Publish
RUN dotnet publish CostumerService.Api/CostumerService.Api.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CostumerService.Api.dll"]
