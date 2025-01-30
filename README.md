# NewJobSurveyAdmin

The New Job Survey Admin tool assists BC Stats in administering the [BCPS New Job Survey](https://www2.gov.bc.ca/gov/content/data/statistics/surveys/new-job-survey).

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)
![img](https://img.shields.io/badge/Lifecycle-Stable-blue)


## Directory Structure

```txt
.github/           - GitHub Actions configuration
backend/           - The .NET Core backend
└── Dockerfile     - The Docker image for the backend
frontend/          - The React frontend
└── Dockerfile     - The Docker image for the frontend
charts/            - Helm charts
└── njsa/          - Helm chart for the frontend
└── njsa-api/      - Helm chart for the backend
```

## Deployment

The frontend and backend components are deployed by GitHub Actions. The frontend is deployed to OpenShift using the `njsa` Helm chart, and the backend is deployed using the `njsa-api` Helm chart.

## Running a Local Development Environment

### Setup

1. Ensure the [.NET Core SDK 8.0.404](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (on the linked page, scroll down and expand the 8.0.XX disclosure arrow to find the correct SDK)
   is installed.
2. Ensure you have the .NET EF Core CLI installed: `dotnet tool install dotnet-ef --version 8.0.11`
3. Ensure the [.NET Core HTTPS development certificate is trusted](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-8.0&tabs=visual-studio%2Clinux-sles#trust-the-aspnet-core-https-development-certificate-on-windows-and-macos): `dotnet dev-certs https --trust`
4. Install [Postgres](https://www.postgresql.org/download/) and create a
   database named `njsa`.
5. Check out the code from this repository.

**NB**. To be fully functional, the application should be run in conjunction
with the CallWeb API. The code for the CallWeb API is not publicly available.
Please reach out to the project team for access. However, the project will still
build and run without the CallWeb API.

6. Install EF Core dependencies

```
   dotnet tool install dotnet-ef --version 8.0.11
   dotnet add package Microsoft.EntityFrameworkCore --version 8.0.11
   dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.11
```

### Config + secret settings

The application uses two `appsettings.json` files, one in the `/backend/config`
directory and one in `/backend/secret`. This mirrors how the application gets
deployed to OpenShift. `/backend/config/appsettings.json` contains non-sensitive
configuration for the application, while `/backend/secret/appsettings.json` contains
configuration that should not readily visible.

See also the comments in [Program.cs](backend/Program.cs) about how the files get
resolved when the application is deployed to OpenShift.

7. Copy the contents of `backend/config/appsettings.config-template.json`
   into a new file, `backend/config/appsettings.json`, and update the values as
   appropriate.

8. Do the same with `backend/secret/appsettings.secret-template.json`, copying it into
   `/secret/appsettings.json`.

9. Update `frontend/config/__ENV.js` with the correct values.

### Run migrations

10. From the `/backend` project directory, run `dotnet ef database update`. This will
   run the migrations and set up your development database.

   ENSURE THAT 
   dotnet add package Microsoft.EntityFrameworkCore --version 8.0.11
   dotnet add package Microsoft.EntityFrameworkCore.design --version 8.0.11

   Note that the database will be seeded automatically when the application is
   started.

   **NB** the migration uses the `ConnectionStrings` section of `backend\secret\appsettings.json` 
   to connect to the database.

### Start the API

11. Open the `backend` code directory in [Visual Studio Code](https://code.visualstudio.com).
   You may be prompted to add required assets and/or resolve dependencies; do
   so.
12. While in Visual Studio Code, press <kbd>CTRL</kbd> + <kbd>F5</kbd> to launch
    the API.
    
13. Test that the API is running correctly by checking the HealthStatus. If
    the project is running at the default location and port:
    `curl http://localhost:5050/api/HealthStatus/Status`.

### Start the frontend

14. From the `/frontend` directory run 
   ```
   yarn install
   yarn start
   ```

   You should see the application open in a new browser.

   **NB** if the browser displays a blank page, verify `frontend/config/__ENV.js` has the correct values (it might have been overwritten to almost empty).

### Quick database reset (dev ONLY)

These commands will quickly drop the database, delete migrations, create an initial migration, and update the database.

```
dotnet ef database drop --force;
rm -rf Migrations/*.cs;
dotnet ef migrations add InitialCreate;
dotnet ef database update
```
