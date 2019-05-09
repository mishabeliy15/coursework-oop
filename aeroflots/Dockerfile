FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
# EXPOSE 80
# EXPOSE 443
EXPOSE 5080

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["aeroflots/aeroflots.csproj", "aeroflots/"]
RUN dotnet restore "aeroflots/aeroflots.csproj"
COPY . .
WORKDIR "/src/aeroflots"
RUN dotnet build "aeroflots.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "aeroflots.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "aeroflots.dll"]
