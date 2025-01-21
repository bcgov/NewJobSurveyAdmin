
# Migration from .NET 3.0 to .NET 7.0

# Package updates:

go to NewJobSurveyAdmin.csproj and update each package to the following versions:

* <TargetFramework>netcoreapp3.1</TargetFramework> => <TargetFramework>net7.0</TargetFramework>
* CsvHelper => Version 30.0.1
* MailKit => Version 4.1.0
* Newtonsoft.Json => 13.0.3
* Microsoft.EntityFrameworkCore => Version="7.0.0"
* Microsoft.EntityFrameworkCore.Design => Version="7.0.0"
* Microsoft.EntityFrameworkCore.InMemory => Version="7.0.0"
* Microsoft.EntityFrameworkCore.SqlServer => Version="7.0.0"
* Microsoft.AspNetCore.Authentication.JwtBearer => Version="7.0.0"
* Novell.Directory.Ldap.NETStandard => Version="3.6.0"
* Npgsql => Version="8.0.0-preview.4"
* Npgsql.EntityFrameworkCore.PostgreSQL => Version="7.0.0"
* Npgsql.EntityFrameworkCore.PostgreSQL.Design => Version="1.1.0"
* Sieve => 3.0.0-beta0015

# SDK update: 

go to global.json and add the following:

"sdk": {
    "version": "7.0.306",
    "rollForward": "latestFeature"
 }

# Library call updates in CsvServices.cs

go to Controllers\Services\Csv\CsvService.cs and make the following changes:

RegisterClassMap has been moved from Configuration to Context

change:

csv.Configuration.RegisterClassMap<PsaCsvMap>();

to:

csv.Context.RegisterClassMap<PsaCsvMap>();

In Version 20.0.0 CsvConfiguration changed to a read only record to eliminate threading issues. You need to create the configuration ahead of time and pass it into CsvReader/CsvWriter

change:

csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.InsideQuotes; 

to: 

var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
	TrimOptions = TrimOptions.Trim
};

change:

csv.Configuration.BadDataFound = context =>
{
	isRecordBad = true;
    badRecords.Add(context.RawRecord);
};

to: 

var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
	TrimOptions = TrimOptions.Trim
    BadDataFound = context => badRecords.Add(context.RawRecord)
};

# launch.json:

go to .vscode/launch.json: 

change:

"program": "${workspaceFolder}/bin/Debug/netcoreapp3.1/ExitSurveyAdmin.dll", 

to: 

"program": "${workspaceFolder}/bin/Debug/net7.0/ExitSurveyAdmin.dll",

# New Minimal Hosting Model

NOTE: using the [new minimal hosting model](https://learn.microsoft.com/en-us/aspnet/core/migration/50-to-60?view=aspnetcore-7.0&tabs=visual-studio#new-hosting-model) for .NET 7.0 is recommended but not required