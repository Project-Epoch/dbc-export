# DBC Exporter

This is a small CLI tool that is able to connect to a MySQL database and export the data from DBC tables into a WotLK 3.3.5 Client DBC file. These are defined in `definitions.json` and is currently an incomplete set. As and when I come across a new DBC that I need while working on projects this will be expanded. The ones currently provided were simply chosen to encompass a broad selection of data types to test the implementation. 

**Supported Cores:** 

- AzerothCore

## Usage

Firstly you must enter your database credentials within an `appsettings.json` file placed alongside the executable. This can be accomplished with `cp appsettings.json.dist appsettings.json`. 

Fill in the following:

- `DB_HOST` The IP or hostname of your database.
- `DB_PORT` The port your database is exposed on.
- `DB_USER` The user to connect with.
- `DB_PASS` The password to use correlating with the above user.
- `WORLD_DB` The name of your world database.

### From Source

1. Ensure that you have installed the [.NET Core SDK x64](https://dotnet.microsoft.com/download) for your OS (Windows, OSX, Linux).
2. From a terminal execute `dotnet restore`.
3. From a terminal execute `dotnet run`.
