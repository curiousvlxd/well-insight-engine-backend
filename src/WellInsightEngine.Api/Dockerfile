FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WellInsightEngine.Api/WellInsightEngine.Api.csproj", "src/WellInsightEngine.Api/"]
COPY ["src/WellInsightEngine.Core/WellInsightEngine.Core.csproj", "src/WellInsightEngine.Core/"]
COPY ["src/WellInsightEngine.Infrastructure/WellInsightEngine.Infrastructure.csproj", "src/WellInsightEngine.Infrastructure/"]
RUN dotnet restore "src/WellInsightEngine.Api/WellInsightEngine.Api.csproj"
COPY . .
WORKDIR "/src/src/WellInsightEngine.Api"
RUN dotnet build "./WellInsightEngine.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WellInsightEngine.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WellInsightEngine.Api.dll"]
