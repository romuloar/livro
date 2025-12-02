# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/Livro.Presentation.Api/Livro.Presentation.Api.csproj", "src/Livro.Presentation.Api/"]
COPY ["src/Livro.Presentation.Hosting/Livro.Presentation.Hosting.csproj", "src/Livro.Presentation.Hosting/"]
COPY ["src/Livro.Application/Livro.Application.csproj", "src/Livro.Application/"]
COPY ["src/Livro.Domain/Livro.Domain.csproj", "src/Livro.Domain/"]
COPY ["src/Livro.Infra.EfCore/Livro.Infra.EfCore.csproj", "src/Livro.Infra.EfCore/"]

RUN dotnet restore "src/Livro.Presentation.Api/Livro.Presentation.Api.csproj"

# Copy all source files
COPY . .

# Build and publish
WORKDIR "/src/src/Livro.Presentation.Api"
RUN dotnet build "Livro.Presentation.Api.csproj" -c Release -o /app/build
RUN dotnet publish "Livro.Presentation.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install SQLite (caso precise de ferramentas CLI)
RUN apt-get update && apt-get install -y sqlite3 && rm -rf /var/lib/apt/lists/*

EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Livro.Presentation.Api.dll"]
