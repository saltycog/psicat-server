# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["PsiCAT.Server.sln", "."]
COPY ["PsiCAT.Server.Core/PsiCAT.Server.Core.csproj", "PsiCAT.Server.Core/"]
COPY ["PsiCAT.Server.DiscordApp/PsiCAT.Server.DiscordApp.csproj", "PsiCAT.Server.DiscordApp/"]

# Restore dependencies
RUN dotnet restore "PsiCAT.Server.sln"

# Copy remaining source code
COPY . .

# Build the solution
RUN dotnet build "PsiCAT.Server.sln" -c Release -o /app/build

# Publish PsiCAT.Server.Core (which includes DiscordApp as dependency)
RUN dotnet publish "PsiCAT.Server.Core/PsiCAT.Server.Core.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Create data directories that will be mounted from host
RUN mkdir -p /app/Data /app/wwwroot/avatars

# Copy published application from build stage
# (daemon files are included in the publish output from Core's Content items)
COPY --from=build /app/publish .

# Expose HTTP and HTTPS ports
EXPOSE 5247 7011

# Set environment variables
ENV ASPNETCORE_URLS=http://0.0.0.0:5247
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "PsiCAT.Server.Core.dll"]
