# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and csproj first for caching
COPY OrderService.sln ./
COPY OrderService.Api/OrderService.Api.csproj OrderService.Api/

# Restore only
RUN dotnet restore OrderService.sln

# Copy the rest of the source code
COPY . .

# Publish
RUN dotnet publish OrderService.Api/OrderService.Api.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
