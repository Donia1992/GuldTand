# Guldtand Backend

This is the repository for the project group **Guldtand**'s dental practice management software system .NET Core Web API.
Our frontend repository is available [here](https://github.com/Donia1992/GuldtandFrontEnd).

### Database updating
For versioning and O/RM we use Entity Framework on a SQL Server database.
In order to create new migrations or to upgrade (or downgrade) your local database use the following commands in the NuGet Package Manager Console.

To scaffold the next migration based on changes you have made to your model since the last migration was created:
```sh
add-migration name-of-migration
```
To migrate to any specific version:
```sh
update-database –TargetMigration: name-of-migration 
```
To apply any pending migrations to the database:
```sh
update-database -v
```
