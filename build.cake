///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var version = "1.0.0.0";

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
   .Does(() =>
{
   CleanDirectories("LINQPad.ABP/bin/" + configuration);
});

Task("Build")
   .IsDependentOn("Clean")
   .Does(() =>
{
   DotNetCoreBuild("LINQPad.ABP.sln", new DotNetCoreBuildSettings {
      Configuration = configuration,
      MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersion(version)
   });
});

Task("Pack")
   .IsDependentOn("Build")
   .Does(() =>
{
   DotNetCorePack("LINQPad.ABP.sln", new DotNetCorePackSettings {
      NoBuild = true,
      IncludeSymbols = true,
      Configuration = configuration,
      MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersion(version)
   });
});

Task("Default")
.Does(() => {
   Information("Hello Cake!");
});

RunTarget(target);