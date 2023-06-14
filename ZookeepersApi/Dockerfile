
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ZookeepersApi/ZookeepersApi.csproj", "ZookeepersApi/"]
COPY ["../MicroZoo.Infrastructure/Infrastructure.csproj", "MicroZoo.Infrastructure/"]
RUN dotnet restore "ZookeepersApi/ZookeepersApi.csproj"
COPY . .
WORKDIR "/src/ZookeepersApi"
RUN dotnet build "ZookeepersApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZookeepersApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZookeepersApi.dll"]