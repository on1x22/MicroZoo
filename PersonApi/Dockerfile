
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PersonApi/PersonsApi.csproj", "PersonApi/"]
COPY ["../MicroZoo.Infrastructure/Infrastructure.csproj", "MicroZoo.Infrastructure/"]
RUN dotnet restore "PersonApi/PersonsApi.csproj"
COPY . .
WORKDIR "/src/PersonApi"
RUN dotnet build "PersonsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PersonsApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonsApi.dll"]