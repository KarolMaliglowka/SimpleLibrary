# ==========================
# Build stage
# ==========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Kopiowanie plików projektów (lepsze wykorzystanie cache)
COPY ["SimpleLibrary.Api/SimpleLibrary.Api.csproj", "SimpleLibrary.Api/"]
COPY ["SimpleLibrary.Application/SimpleLibrary.Application.csproj", "SimpleLibrary.Application/"]
COPY ["SimpleLibrary.Infrastructure/SimpleLibrary.Infrastructure.csproj", "SimpleLibrary.Infrastructure/"]
COPY ["SimpleLibrary.Core/SimpleLibrary.Core.csproj", "SimpleLibrary.Core/"]

RUN dotnet restore "SimpleLibrary.Api/SimpleLibrary.Api.csproj"

# Kopiowanie całego rozwiązania
COPY . .

WORKDIR "/src/SimpleLibrary.Api"

RUN dotnet publish -c Release -o /app/publish --no-restore

# ==========================
# Runtime stage
# ==========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "SimpleLibrary.Api.dll"]
