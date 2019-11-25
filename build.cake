#load "solution-utility.cake"

var target = Argument("target", "Default");
var currentVersion = Argument("build-version", "");
var parameters = SolutionUtility.GetParameters("pepe.json");
parameters.Log();
var version = parameters.Version;

Task("Default")
.Does((context) =>
{
    Information("Creating build with version: {0}", currentVersion);
    var parameters = SolutionUtility.GetParameters("pepe.json");
    Information("Version is {0}", parameters.Version);
});

Task("create-win-pdb-files")
.Does((context) => 
{
    var winFilesRootPath = "win/bin/Release";
    var winx64FileRootPath = "win/bin.x64/Release";

    var winFiles = System.IO.Directory.GetFiles(winFilesRootPath, "*.pdb", SearchOption.AllDirectories).ToArray();
    context.Zip(winFilesRootPath, $"/acrm-win32-pdb-files.{version}.zip", winFiles);

    var winx64Files = System.IO.Directory.GetFiles(winx64FileRootPath, "*.pdb", SearchOption.AllDirectories).ToArray();
    context.Zip(winx64FileRootPath, $"/acrm-x64-pdb-files.{version}.zip", winx64Files);
});

Task("create-net-pdb-files")
.Does((context) => 
{
    var netFilesRootPath = "net/solutions/output/Release";

    var netFiles = System.IO.Directory.GetFiles(netFilesRootPath, "*.pdb", SearchOption.AllDirectories).ToArray();
    context.Zip(netFilesRootPath, $"/acrm-net-pdb-files.{version}.zip", netFiles);
});


RunTarget(target);