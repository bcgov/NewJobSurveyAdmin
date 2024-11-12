# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app

# Copy everything and restore as distinct layers
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Stage 2: Build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
WORKDIR /opt/app-root/app
COPY --from=build /out .

# Set the entry point to your application
ENTRYPOINT ["dotnet", "NewJobSurveyAdmin.dll"]
