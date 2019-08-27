FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["RoomManager/RoomManager.csproj", "RoomManager/"]
RUN dotnet restore "RoomManager/RoomManager.csproj"
COPY . .
WORKDIR "/src/RoomManager"
RUN dotnet build "RoomManager.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RoomManager.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RoomManager.dll"]