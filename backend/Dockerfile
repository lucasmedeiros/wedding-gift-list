FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN mkdir -p /opt/wedding-gift-api/data
RUN chmod -R 777 /opt/wedding-gift-api/data

COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "WeddingGiftList.Api.dll"]
