FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/MovieService/MovieService.csproj", "Services/MovieService/"]
COPY ["MovieReviewApp.CommonModels/MovieReviewApp.Common.csproj", "MovieReviewApp.CommonModels/"]
RUN dotnet restore "./Services/MovieService/./MovieService.csproj"
COPY . .
WORKDIR "/src/Services/MovieService"
RUN dotnet build "./MovieService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MovieService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieService.dll"]