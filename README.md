# NewJobSurveyAdmin

The New Job Survey Admin tool will assist BC Stats in administering the BCPS New Job Survey.

# Development tasks

## Running a development environment

1. Ensure the [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) is installed.
2. Ensure the [.NET Core HTTPS development certificate is trusted](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio#trust-the-aspnet-core-https-development-certificate-on-windows-and-macos).
3. Install [Postgres](https://www.postgresql.org/download/) and create a database named `NewJobSurveyAdmin`.
4. Check out the code from this repository.
5. Update the connection string to your local Postgres instance in `appsettings.Development.json`, along the following lines:
```
  "ConnectionStrings": {
    "NewJobSurveyAdmin": "Server=127.0.0.1,1433;Database=NewJobSurveyAdmin;User Id=sa;Password=Y0urP4ssw0rd"
  }
```
6. From the root project directory, run `dotnet ef database update`. This will run the migrations and set up your development database.
7. On the command line / terminal, from the `ClientApp` directory (in the root project directory), run `yarn install`.
8. Still in the `ClientApp` directory, run `yarn start` to launch the front-end.
9. Open the checked-out code in [Visual Studio Code](https://code.visualstudio.com).
10. While in Visual Studio Code, press <kbd>CTRL</kbd> + <kbd>F5</kbd> to launch the API.

## Quick database reset (dev ONLY)

This command will quickly drop the database, delete migrations, create an initial migration, and update the database.

```
dotnet ef database drop --force;rm -rf Migrations/*.cs;dotnet ef migrations add InitialCreate;dotnet ef database update
```