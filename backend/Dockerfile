# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy everything and restore as distinct layers
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Stage 2: Build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Set the entry point to your application
ENTRYPOINT ["dotnet", "NewJobSurveyAdmin.dll"]
