#I "Src/packages/FAKE.3.13.3/tools"
#r "FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.EnvironmentHelper
open Fake.MSBuildHelper
open Fake.NuGetHelper
open Fake.XUnitHelper

// properties
let semanticVersion = "2.0.3"
let version = (>=>) @"(?<major>\d*)\.(?<minor>\d*)\.(?<build>\d*).*?" "${major}.${minor}.${build}.0" semanticVersion
let configuration = getBuildParamOrDefault "configuration" "Release"
let deployToNuGet = getBuildParamOrDefault "deployToNuGet" "false"
let genDir = "Gen/"
let docDir = "Doc/"
let srcDir = "Src/"
let testDir = genDir @@ "Test"
let nugetDir = genDir @@ "NuGet"

Target "Clean" (fun _ ->
    CleanDirs[genDir; testDir; nugetDir]

    build (fun p ->
        { p with
            Verbosity = Some(Quiet)
            Targets = ["Clean"]
            Properties = ["Configuration", configuration]
        })
        (srcDir @@ "HelperTrinity.sln")
)

Target "Build" (fun _ ->
    // generate the shared assembly info
    CreateCSharpAssemblyInfoWithConfig (srcDir @@ "AssemblyInfoCommon.cs")
        [
            Attribute.Version version
            Attribute.FileVersion version
            Attribute.Configuration configuration
            Attribute.Company "Kent Boogaart"
            Attribute.Product "The Helper Trinity"
            Attribute.Copyright "© Copyright. Kent Boogaart."
            Attribute.Trademark ""
            Attribute.Culture ""
            Attribute.CLSCompliant true
            Attribute.StringAttribute("NeutralResourcesLanguage", "en-US", "System.Resources")
            Attribute.StringAttribute("AssemblyInformationalVersion", semanticVersion, "System.Reflection")
        ]
        (AssemblyInfoFileConfig(false))

    build (fun p ->
        { p with
            Verbosity = Some(Quiet)
            Targets = ["Build"]
            Properties =
                [
                    "Optimize", "True"
                    "DebugSymbols", "True"
                    "Configuration", configuration
                ]
        })
        (srcDir @@ "HelperTrinity.sln")
)

Target "ExecuteUnitTests" (fun _ ->
    xUnit (fun p ->
        { p with
            ShadowCopy = false;
            HtmlOutput = true;
            XmlOutput = true;
            OutputDir = testDir
        })
        [srcDir @@ "Kent.Boogaart.HelperTrinity.UnitTests/bin" @@ configuration @@ "Kent.Boogaart.HelperTrinity.UnitTests.dll"]
)

Target "CreateArchives" (fun _ ->
    // source archive
    !! "**/*.*"
        -- ".git/**"
        -- (genDir @@ "**")
        -- (srcDir @@ "packages/**/*")
        -- (srcDir @@ "**/*.suo")
        -- (srcDir @@ "**/*.csproj.user")
        -- (srcDir @@ "**/*.gpState")
        -- (srcDir @@ "**/bin/**")
        -- (srcDir @@ "**/obj/**")
        |> Zip "." (genDir @@ "TheHelperTrinity-" + semanticVersion + "-src.zip")

    // binary archive
    let workingDir = srcDir @@ "Kent.Boogaart.HelperTrinity/bin" @@ configuration

    !! (workingDir @@ "Kent.Boogaart.HelperTrinity.*")
        |> Zip workingDir (genDir @@ "TheHelperTrinity-" + semanticVersion + "-bin.zip")
)

Target "CreateNuGetPackages" (fun _ ->
    // copy binaries to lib
    !! (srcDir @@ "Kent.Boogaart.HelperTrinity/bin" @@ configuration @@ "Kent.Boogaart.HelperTrinity.*")
        |> CopyFiles (nugetDir @@ "lib/portable-win+net403+sl40+wp+xbox40+MonoAndroid+Xamarin.iOS10+MonoTouch")

    // copy source to src
    [!! (srcDir @@ "**/*.*")
        -- (srcDir @@ "packages/**/*")
        -- (srcDir @@ "**/*.suo")
        -- (srcDir @@ "**/*.csproj.user")
        -- (srcDir @@ "**/*.gpState")
        -- (srcDir @@ "**/bin/**")
        -- (srcDir @@ "**/obj/**")]
        |> CopyWithSubfoldersTo nugetDir

    // create the NuGets
    NuGet (fun p ->
        {p with
            Project = "Kent.Boogaart.HelperTrinity"
            Version = semanticVersion
            OutputPath = nugetDir
            WorkingDir = nugetDir
            SymbolPackage = NugetSymbolPackage.Nuspec
            Publish = System.Convert.ToBoolean(deployToNuGet)
        })
        (srcDir @@ "HelperTrinity.nuspec")
)

// build order
"Clean"
    ==> "Build"
    ==> "ExecuteUnitTests"
    ==> "CreateArchives"
    ==> "CreateNuGetPackages"

RunTargetOrDefault "CreateNuGetPackages"