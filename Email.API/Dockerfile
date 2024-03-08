FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Email.API/Email.API.csproj", "Email.API/"]
COPY ["Email.Application/Email.Application.csproj", "Email.Application/"]
RUN dotnet restore "Email.API/Email.API.csproj"
COPY . .
WORKDIR "/src/Email.API"
RUN dotnet build "Email.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Email.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Email.API.dll"]
