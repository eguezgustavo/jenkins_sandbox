#addin "Cake.FileHelpers"

var target = Argument("target", "Default");
var version = Argument("build-version", "");


Task("Default")
.Does((context) =>
{
    ReplaceRegexInFiles("./pepe.json", "\"version\": \"(.*?)\"", string.Format("\"version\": \"{0}\"", version));
});

RunTarget(target);