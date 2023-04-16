#tool "nuget:?package=NuGet.CommandLine&version=6.5.0"

#addin "nuget:?package=Cake.MinVer&version=3.0.0"
#addin "nuget:?package=Cake.Args&version=3.0.0"

using System.Net.Http;

var target       = ArgumentOrDefault<string>("target") ?? "pack";
var buildVersion = MinVer(s => s.WithTagPrefix("v").WithDefaultPreReleasePhase("preview"));

Task("clean")
    .Does(() =>
{
    CleanDirectories("./artifact/**");
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
        OutputDirectory = "./artifact/nuget",
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

    var nugetPushSettings = new DotNetNuGetPushSettings
    {
        Source = url,
        ApiKey = apiKey,
    };

    foreach (var nugetPackageFile in GetFiles("./artifact/nuget/*.nupkg"))
    {
        DotNetNuGetPush(nugetPackageFile.FullPath, nugetPushSettings);
    }
});

RunTarget(target);
