FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN ls
COPY ["src/BtcTurk.Order.Api/BtcTurk.Order.Api.csproj", "BtcTurk.Order.Api/"]
RUN dotnet restore "src/BtcTurk.Order.Api/BtcTurk.Order.Api.csproj"
COPY . .
WORKDIR "/src/BtcTurk.Order.Api"
RUN dotnet build "BtcTurk.Order.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BtcTurk.Order.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BtcTurk.Order.Api.dll"]
