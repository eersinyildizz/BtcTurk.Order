dotnet publish src/BtcTurk.Order.Api/BtcTurk.Order.Api.csproj --os linux --arch x64 /t:PublishContainer -c Release
docker-compose up -d