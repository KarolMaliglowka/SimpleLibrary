# ==========================
# Build stage
# ==========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Kopiowanie plików projektów (lepsze wykorzystanie cache)
COPY ["src/Backend/Library.Api/Library.Api.csproj", "src/Backend/Library.Api/"]
COPY ["src/Backend/Library.Application/Library.Application.csproj", "src/Backend/Library.Application/"]
COPY ["src/Backend/Library.Infrastructure/Library.Infrastructure.csproj", "src/Backend/Library.Infrastructure/"]
COPY ["src/Backend/Library.Core/Library.Core.csproj", "src/Backend/Library.Core/"]

RUN dotnet restore "src/Backend/Library.Api/Library.Api.csproj"

# Kopiowanie całego rozwiązania
COPY . .

WORKDIR "/src/Backend/Library.Api"

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
