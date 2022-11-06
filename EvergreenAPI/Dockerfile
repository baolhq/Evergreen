
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EvergreenAPI/EvergreenAPI.csproj", "EvergreenAPI/"]
RUN dotnet restore "EvergreenAPI/EvergreenAPI.csproj"
COPY . .
RUN dotnet publish "EvergreenAPI.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EvergreenAPI.dll"]