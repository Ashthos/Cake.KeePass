// To create the bootstrapper, from within powershell run the following.
// Invoke-WebRequest http://cakebuild.net/bootstrapper/windows -OutFile build.ps1
//
// To execute, run the following within powershell
// ./Build.ps1 -Target "build"

// For smoke test Task
// #Addin "Cake.KeePass"

// If testing a local dll, comment out the #Addin above and uncomment the line below.
#r "src/Cake.KeePass/bin/Release/netstandard2.0/Cake.KeePass.dll"

#tool "nuget:?package=xunit.runner.console"

    const string Configuration = "Release";

    var target = Argument("target", "Build");
    var nugetApiKey = Argument<string>("nugetApi", EnvironmentVariable("NugetApiKey"));
    var nugetSource = Argument<string>("nugetSource", "https://www.nuget.org/api/v2/package");

    var solutionFile = File("./Cake.KeePass.sln");
    var artifactsDir = Directory("./artifacts");
    var nupkgDestDir = artifactsDir + Directory("nuget-package");


    Task("Clean")
    .Does(() => {
        CleanDirectories(new DirectoryPath[] {
            artifactsDir,
            nupkgDestDir
        });

		try {
			CleanDirectories("./**/bin/**");
		} catch (Exception exception) {
			Warning("Failed to clean one or more directories.");
		}

    });

    Task("Build")
    .IsDependentOn("Clean")
    .Does(() => {

        Information("Restoring Nuget Packages");
        DotNetCoreRestore(solutionFile);
            
        var settings = new DotNetCoreBuildSettings();
        settings.Configuration = Configuration;
            
        Information("Compiling Solution");
        DotNetCoreBuild(solutionFile, settings);

    });
    
    Task("Integration-Test")
    .IsDependentOn("Build")
    .Does(() => {

        Information("Running KeePass integration tests");
        XUnit2("./test/integration/**/bin/**/*.Test.Integration.dll");

    }); 

    Task("Smoke-Test")
    .Does(() => {
        
        Information("Attempting Password retrieval.");

        var dbSettings = new KeePassDatabaseSettings {
            DatabasePath = "./test/IntegrationTestDatabase.kdbx",
            MasterPassword = "Pass@word!"
        };

        var entryCriteria = new KeePassEntryCriteria {
            GroupHierarchy = new []{ "One", "Two", "Three" },
            Uuid = "B0160DB1A014994E996D0D18836BDE4D"
        };

        var entry = KeePassReadEntry(dbSettings, entryCriteria);

        Information("Entry Password is: " + entry.Password);

    }); 

    Task("Package")
    .Does(() => {
	
		var nuGetPackSettings   = new NuGetPackSettings {
		Version                 = "1.0.0",
		BasePath                = "./",
		OutputDirectory         = nupkgDestDir
		};

		NuGetPack(File("Cake.KeePass.nuspec"), nuGetPackSettings);

    });
    
    Task("Publish")
        .IsDependentOn("Package")
        .Does(() => {
        
            var packages = GetFiles(nupkgDestDir.Path + "/Cake.KeePass.*.nupkg");
                
            foreach (var package in packages) {    
                // Push the package.
                NuGetPush(package, new NuGetPushSettings {
                    Source = nugetSource,
                    ApiKey = nugetApiKey
                    });
            }
    });

    RunTarget(target);
