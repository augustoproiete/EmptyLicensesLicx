#tool "nuget:?package=NuGet.CommandLine&version=5.11.0"

#addin "nuget:?package=Cake.MinVer&version=2.0.0"
#addin "nuget:?package=Cake.Args&version=1.0.1"

using System.Net.Http;

var target       = ArgumentOrDefault<string>("target") ?? "pack";
var buildVersion = MinVer(s => s.WithTagPrefix("v").WithDefaultPreReleasePhase("preview"));

Task("clean")
    .Does(() =>
{
    CleanDirectories("./artifacts/**");
});

Task("pack")
    .IsDependentOn("clean")
    .Does(() =>
{
    Information("Creating NuGet package");

    var nuspecFilePath = "./src/EmptyLicensesLicx.nuspec";

    NuGetPack(nuspecFilePath, new NuGetPackSettings
    {
        Version = buildVersion.PackageVersion,
        OutputDirectory = "./artifacts/nuget",
    });
});

Task("push")
    .IsDependentOn("pack")
    .Does(context =>
{
    Information("Publishing NuGet package");

    var url =  context.EnvironmentVariable("NUGET_URL");
    if (string.IsNullOrWhiteSpace(url))
    {
        context.Information("No NuGet URL specified. Skipping publishing of NuGet packages");
        return;
    }

    var apiKey =  context.EnvironmentVariable("NUGET_API_KEY");
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        context.Information("No NuGet API key specified. Skipping publishing of NuGet packages");
        return;
    }

    var nugetPushSettings = new DotNetCoreNuGetPushSettings
    {
        Source = url,
        ApiKey = apiKey,
    };

    foreach (var nugetPackageFile in GetFiles("./artifacts/nuget/*.nupkg"))
    {
        DotNetCoreNuGetPush(nugetPackageFile.FullPath, nugetPushSettings);
    }
});

RunTarget(target);
