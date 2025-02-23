# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RAG.AI.Presenter.API/RAG.AI.Presenter.API.csproj", "RAG.AI.Presenter.API/"]
COPY ["RAG.AI.Application/RAG.AI.Application.csproj", "RAG.AI.Application/"]
COPY ["RAG.AI.Infrastructure/RAG.AI.Infrastructure.csproj", "RAG.AI.Infrastructure/"]
COPY ["RAG.AI.Domain/RAG.AI.Domain.csproj", "RAG.AI.Domain/"]
COPY ["RAG.AI.BackgroundTasks/RAG.AI.BackgroundTasks.csproj", "RAG.AI.BackgroundTasks/"]
RUN dotnet restore "./RAG.AI.Presenter.API/RAG.AI.Presenter.API.csproj"
COPY . .
WORKDIR "/src/RAG.AI.Presenter.API"
RUN dotnet build "./RAG.AI.Presenter.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RAG.AI.Presenter.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RAG.AI.Presenter.API.dll"]
