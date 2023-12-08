FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NewClient/NewClient.csproj", "NewClient/"]
RUN dotnet restore "NewClient/NewClient.csproj"
COPY . .
WORKDIR "/src/NewClient"
RUN dotnet build "NewClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NewClient.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewClient.dll"]
