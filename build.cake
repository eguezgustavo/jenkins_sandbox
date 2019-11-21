var target = Argument("target", "Default");
var currentVersion = Argument("build-version", "");


Task("Default")
.Does((context) =>
{
    
});

RunTarget(target);