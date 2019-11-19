var target = Argument("target", "Default");
var version = Argument("build_version", "");

Task("Default")
.Does(() =>
{
  Information("Build Version: {0} ", version);
});

RunTarget(target);