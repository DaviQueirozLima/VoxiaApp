# =============================
# Etapa base (runtime)
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# =============================
# Etapa de build
# =============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os .csproj de todas as camadas
COPY ["src/Voxia.Api/Voxia.Api.csproj", "src/Voxia.Api/"]
COPY ["src/Voxia.Application/Voxia.Application.csproj", "src/Voxia.Application/"]
COPY ["src/Voxia.Domain/Voxia.Domain.csproj", "src/Voxia.Domain/"]
COPY ["src/Voxia.Infrastructure/Voxia.Infrastructure.csproj", "src/Voxia.Infrastructure/"]

# Restaura dependências
RUN dotnet restore "./src/Voxia.Api/Voxia.Api.csproj"

# Copia o restante do código
COPY . .

# Compila
WORKDIR "/src/src/Voxia.Api"
RUN dotnet build "./Voxia.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# =============================
# Etapa de publish
# =============================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Voxia.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# =============================
# Etapa final (produção)
# =============================
FROM base AS final
WORKDIR /app

# Copia a publicação final
COPY --from=publish /app/publish .

# Copia também a pasta de Assets (áudios e imagens)
COPY src/Voxia.Api/Assets ./Assets

# Entry point
ENTRYPOINT ["dotnet", "Voxia.Api.dll"]
