# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/TeamProject.WebApi/TeamProject.WebApi.csproj", "src/Presentation/TeamProject.WebApi/"]
COPY ["src/Infrastructure/TeamProject.Infrastructure/TeamProject.Infrastructure.csproj", "src/Infrastructure/TeamProject.Infrastructure/"]
COPY ["src/Core/TeamProject.Application/TeamProject.Application.csproj", "src/Core/TeamProject.Application/"]
COPY ["src/Core/TeamProject.Domain/TeamProject.Domain.csproj", "src/Core/TeamProject.Domain/"]
COPY ["src/Infrastructure/TeamProject.Persistence/TeamProject.Persistence.csproj", "src/Infrastructure/TeamProject.Persistence/"]
RUN dotnet restore "./src/Presentation/TeamProject.WebApi/TeamProject.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Presentation/TeamProject.WebApi"
RUN dotnet build "./TeamProject.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TeamProject.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamProject.WebApi.dll"]