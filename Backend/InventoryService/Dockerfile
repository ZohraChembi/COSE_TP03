FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY OrderService.sln ./
COPY OrderService.Api/OrderService.Api.csproj OrderService.Api/

RUN dotnet restore OrderService.sln

COPY . .

RUN dotnet publish OrderService.Api/OrderService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
