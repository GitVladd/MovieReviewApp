#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/ReviewService/ReviewService.csproj", "Services/ReviewService/"]
COPY ["MovieReviewApp.Common/MovieReviewApp.Common.csproj", "MovieReviewApp.Common/"]
RUN dotnet restore "./Services/ReviewService/./ReviewService.csproj"
COPY . .
WORKDIR "/src/Services/ReviewService"
RUN dotnet build "./ReviewService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ReviewService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReviewService.dll"]


## Base image for running the app
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
## SDK image for building the app
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["Services/ReviewService/ReviewService.csproj", "Services/ReviewService/"]
#COPY ["MovieReviewApp.Common/MovieReviewApp.Common.csproj", "MovieReviewApp.Common/"]
#RUN dotnet restore "./Services/ReviewService/./ReviewService.csproj"
#COPY . .
#WORKDIR "/src/Services/ReviewService"
#RUN dotnet build "./ReviewService.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## Publish the app
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./ReviewService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## Final image - combining base image with SDK for migration support
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#
## Use ENTRYPOINT to run the app
#ENTRYPOINT ["dotnet", "ReviewService.dll"]