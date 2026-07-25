# ==========================
# Build stage
# ==========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Kopiowanie plików projektów (lepsze wykorzystanie cache)
COPY ["Library.Api/Library.Api.csproj", "Library.Api/"]
COPY ["Library.Application/Library.Application.csproj", "Library.Application/"]
COPY ["Library.Infrastructure/Library.Infrastructure.csproj", "Library.Infrastructure/"]
COPY ["Library.Core/Library.Core.csproj", "Library.Core/"]

RUN dotnet restore "Library.Api/Library.Api.csproj"

# Kopiowanie całego rozwiązania
COPY . .

WORKDIR "/src/Library.Api"

RUN dotnet publish -c Release -o /app/publish --no-restore

# ==========================
# Runtime stage
# ==========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5280
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5280

ENTRYPOINT ["dotnet", "Library.Api.dll"]
