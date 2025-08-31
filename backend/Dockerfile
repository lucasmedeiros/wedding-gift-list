# Use the official .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy source code and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish --no-restore

# Use the official .NET 8 runtime for final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create data directory for SQLite database
RUN mkdir -p /app/data

# Copy published application
COPY --from=build /app/publish .

# Set up environment
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5001
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/wedding_gifts.db"

# Expose port
EXPOSE 5001

# Create non-root user for security
RUN adduser --disabled-password --gecos "" --uid 1000 appuser && \
    chown -R appuser:appuser /app && \
    chmod -R 755 /app/data

USER appuser

ENTRYPOINT ["dotnet", "WeddingGiftList.Api.dll"]
