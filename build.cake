var target = Argument("target", "Default");
var currentVersion = Argument("build-version", "");


Task("Default")
.Does((context) =>
{
    Information("Creating build with version: {0}", currentVersion);  
});


RunTarget(target);