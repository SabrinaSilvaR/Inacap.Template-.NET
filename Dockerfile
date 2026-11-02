# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY Directory.Build.props Directory.Packages.props ./
COPY src/FastEndpointsTemplate.Domain/*.csproj ./src/FastEndpointsTemplate.Domain/
COPY src/FastEndpointsTemplate.Application/*.csproj ./src/FastEndpointsTemplate.Application/
COPY src/FastEndpointsTemplate.Infrastructure/*.csproj ./src/FastEndpointsTemplate.Infrastructure/
COPY src/FastEndpointsTemplate.Api/*.csproj ./src/FastEndpointsTemplate.Api/

RUN dotnet restore ./src/FastEndpointsTemplate.Api/FastEndpointsTemplate.Api.csproj

COPY src/ ./src/
RUN dotnet publish ./src/FastEndpointsTemplate.Api/FastEndpointsTemplate.Api.csproj -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FastEndpointsTemplate.Api.dll"]
