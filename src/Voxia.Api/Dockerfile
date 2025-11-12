# =============================
# Base image (runtime)
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# =============================
# Build image
# =============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os .csproj de cada camada
COPY ["src/Voxia.Api/Voxia.Api.csproj", "src/Voxia.Api/"]
COPY ["src/Voxia.Application/Voxia.Application.csproj", "src/Voxia.Application/"]
COPY ["src/Voxia.Domain/Voxia.Domain.csproj", "src/Voxia.Domain/"]
COPY ["src/Voxia.Infrastructure/Voxia.Infrastructure.csproj", "src/Voxia.Infrastructure/"]

# Restaura dependências
RUN dotnet restore "./src/Voxia.Api/Voxia.Api.csproj"

# Copia tudo
COPY . .

WORKDIR "/src/src/Voxia.Api"

# Compila o projeto
RUN dotnet build "./Voxia.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# =============================
# Publish image
# =============================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Voxia.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# =============================
# Final image
# =============================
FROM base AS final
WORKDIR /app

# Copia a build final
COPY --from=publish /app/publish .

# ?? Copia a pasta de Assets pro container
COPY src/Voxia.Api/Assets /app/Assets