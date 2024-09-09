# Use ASP.NET runtime image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use .NET SDK image for the build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["MovieReviewApp.Common/MovieReviewApp.Common.csproj", "MovieReviewApp.Common/"]
COPY ["Services/MovieService/MovieService.csproj", "Services/MovieService/"]
RUN dotnet restore "Services/MovieService/MovieService.csproj"

# Copy the remaining source code and build the project
COPY . .
WORKDIR "/src/Services/MovieService"
RUN dotnet build "MovieService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project to the /app/publish directory
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MovieService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the base image and copy the published files
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieService.dll"]
