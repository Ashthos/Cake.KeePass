# Cake.KeePass

This is a thin wrapper around the KeePassLib library, allowing searching and retrieval of KeePass database entries.

## Usage 

The intention of this Cake plugin is to encourage development teams to store their passwords in a vault rather than checked into source control. 

The plugin has been kept very simple/single responsibility, in order to allow its use in as many situations as possible. The plugin will never attempt to write the retrieved data to configuration files, environment variables or anywhere else - this is for the script writer to perform using one of the other more suitable cake plugins.

## Best Practises

The idea of this plugin is to get passwords out of source control. However, as a plain text master password must be supplied to open a KeePass database, you must store it in an accessible location.

### Private Build Server

- Stored in an environment variable accessible to the build runner.

### Continuous Integration

- Set via a build specific environment variable. 

## Example

```csharp
#Addin "Cake.KeePass"

Information("Attempting Password retrieval.");

var masterPassword = "my-master-password"; // Don't do this! Read it from an env variable or inject it to the script somehow.

var dbSettings = new KeePassDatabaseSettings {
    DatabasePath = "./my-password-database.kdbx",
    MasterPassword = masterPassword
};

var entryCriteria = new KeePassEntryCriteria {
    GroupHierarchy = new []{ "Databases", "SQL Server" },
    Title = "AdministrationDb-Prod"
};

var entry = KeePassReadEntry(dbSettings, entryCriteria);

Information("Entry Password is: " + entry.Password);
```

### Contributing to Cake.KeePass

If you have discovered a bug, or have a feature suggestion, then by far the easiest way to progress is to Fork the Ashthos/Cake.KeePass repository and implement the fix/feature. 