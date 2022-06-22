FROM mcr.microsoft.com/dotnet/nightly/sdk:6.0 AS build-env
WORKDIR /source

# Copy everything
COPY *.csproj ./
# Restore as distinct layers
RUN dotnet restore

COPY . ./
# Build and publish a release
RUN dotnet publish -c release -o /WaveShopAPIRest

# Build runtime image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:6.0
WORKDIR /WaveShopAPIRest
EXPOSE 80
COPY --from=build-env /WaveShopAPIRest ./

ENTRYPOINT ["dotnet", "WaveShopAPIRest.dll"]
