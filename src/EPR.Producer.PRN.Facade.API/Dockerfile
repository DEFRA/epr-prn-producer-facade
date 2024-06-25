#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM defradigital/dotnetcore-development:dotnet8.0 AS base
USER app
WORKDIR /app

ARG PORT=8080
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

FROM defradigital/dotnetcore-development:dotnet8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EPR.Producer.PRN.Facade.API/EPR.Producer.PRN.Facade.API.csproj", "EPR.Producer.PRN.Facade.API/"]
RUN dotnet restore "./EPR.Producer.PRN.Facade.API/EPR.Producer.PRN.Facade.API.csproj"


COPY "EPR.Producer.PRN.Facade.API." "./EPR.Producer.PRN.Facade.API/."
WORKDIR "/src/EPR.Producer.PRN.Facade.API"
RUN dotnet build "./EPR.Producer.PRN.Facade.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EPR.Producer.PRN.Facade.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPR.Producer.PRN.Facade.API.dll"]