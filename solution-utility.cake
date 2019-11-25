using System.Dynamic;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel;

public static class SolutionUtility
{
	private static int NextTimeStampServer = 0;
	public static bool SignDll(ICakeContext context, string certificatePath, string password, string filePath, BuildParameters parameters)
	{
		var returnValue = 1;
		var timeStampServerUrls = parameters.TimestampServerUrls;
		NextTimeStampServer %= timeStampServerUrls.Length;
		(timeStampServerUrls[0], timeStampServerUrls[NextTimeStampServer]) = (timeStampServerUrls[NextTimeStampServer], timeStampServerUrls[0]);

		foreach (string timestampUrl in timeStampServerUrls)
		{
			returnValue = ProcessAliases.StartProcess(context,parameters.SignTool, new ProcessSettings
			{
				Arguments = $"sign /f \"{certificatePath}\" /p {password} /tr \"{timestampUrl}\" " +
					$"/du http://www.aurea.com \"{filePath}\"",
				WorkingDirectory = "./",
			});

			if (returnValue != 0)
			{
				++NextTimeStampServer;
				context.Warning($"Unable to sign {filePath} using server {timestampUrl}.");
				continue;
			}

			returnValue = ProcessAliases.StartProcess(context,parameters.SignTool,new ProcessSettings
			{
				Arguments = $"sign /f \"{certificatePath}\" /p {password} /tr \"{timestampUrl}\" " +
					$"/td SHA256 /fd SHA256 /as /du http://www.aurea.com \"{filePath}\"",
				WorkingDirectory = "./",
			});

			if (returnValue == 0) break;
			++NextTimeStampServer;
			context.Warning($"Unable to sign {filePath} using server {timestampUrl}.");
		}
		return returnValue == 0;
	}

	public static BuildParameters GetParameters(string path)
	{
		var json = System.IO.File.ReadAllText(path);
		return JsonConvert.DeserializeObject<BuildParameters>(json);
	}

	public static void PatchAssemblyVersion(string version, string path)
	{
		var v1 = $"[assembly: AssemblyVersion(\"{version}\")]";
		var v2 = $"[assembly: AssemblyFileVersion(\"{version}\")]";

		var files = System.IO.Directory.GetFiles(path, "AssemblyInfo.cs", SearchOption.AllDirectories);
		foreach (var file in files)
		{
			var lines = System.IO.File.ReadAllLines(file)
				.Where(x => !x.Contains("AssemblyVersion") && !x.Contains("AssemblyFileVersion"))
				.Select(x =>
				{
					if (x.Contains("AssemblyCompany"))
						return "[assembly: AssemblyCompany(\"Aurea, Inc.\")]";
					if (x.Contains("AssemblyCopyright"))
						return $"[assembly: AssemblyCopyright(\"Copyright (c) 1988-{DateTime.Now.Year} Aurea, Inc.\")]";

					return x;
				})
				.ToArray().Concat(new[] { v1, v2 });
			System.IO.File.WriteAllLines(file, lines);
			Console.WriteLine($"Patched {file} version information.");
		}
	}

	public static string CombinePath(string path1, params string[] paths)
	{
		var result = path1;
		foreach(var path in paths)
		{
			result = System.IO.Path.Combine(result, path);
		}
		return result;
	}

	public static void CollectFiles(ICakeContext context, string artifactsPath, string folder, string vertical, string jsonFile, string exclude = null)
	{
		var excludes = new HashSet<string>();
		if (!string.IsNullOrWhiteSpace(exclude))
		{
			excludes = exclude.Split(',').Select(x => x.Trim()).ToHashSet();
		}

		var json = System.IO.File.ReadAllText(jsonFile);
		var filesList = JsonConvert.DeserializeObject<List<string[]>>(json);

		foreach(var filePair in filesList)
		{
			filePair[0] = filePair[0].Replace("{vertical}", vertical);
			filePair[1] = filePair[1].Replace("{vertical}", vertical);
		}

		var path = System.IO.Path.Combine(artifactsPath, folder);
		if (System.IO.Directory.Exists(path))
			context.DeleteDirectory(path, new DeleteDirectorySettings { Recursive = true, Force = true} );

		context.CreateDirectory(path);

		filesList = filesList
			.Where(x => !excludes
				.Contains(System.IO.Path.GetFileName(x[0])))
			.ToList();

		context.Information($"Copying files ({filesList.Count})...");
		foreach(var filePair in filesList)
		{
			var dest = CombinePath(path, filePair[0]);
			var destinationFileName = System.IO.Path.GetFileName(dest);

			var src = CombinePath(filePair[1], destinationFileName);

			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(dest));
			Console.WriteLine(src + " -> " + dest);
			context.CopyFile(src, dest);
		}
		context.Information($"Copied {filesList.Count} files.");
	}

	public static void ZipFolder(ICakeContext context, string source, string destination)
	{
		if(context.FileExists(destination))
			context.DeleteFile(destination);
		var files = System.IO.Directory.GetFiles(source, "*", SearchOption.AllDirectories).ToArray();
		context.Information($"Creating zip package from folder {source} that contains {files.Length} files.");
		context.Zip(source, destination, files);
		context.Information($"Zip package {destination} that contains {files.Length} files is created.");
	}
}

public class BuildParameters
{
	[JsonProperty("version")]
	public string Version { get; set; }

	[JsonProperty("solution")]
	public string Solution { get; set; }

	[JsonProperty("sign-tool")]
	public string SignTool { get; set; }

	[JsonProperty("artifacts-path")]
	public string ArtifactsPath { get; set; }

	[JsonProperty("ms-build-path")]
	public string MSBuildPath { get; set; }

	[JsonProperty("patch-assembly-version")]
	public bool PatchAssemblyVersion { get; set; }

	[JsonProperty("build-release-x64")]
	public bool BuildReleaseX64 { get; set; }

	[JsonProperty("build-release-x86")]
	public bool BuildReleaseX86 { get; set; }

	[JsonProperty("build-debug-x64")]
	public bool BuildDebugX64 { get; set; }

	[JsonProperty("build-debug-x86")]
	public bool BuildDebugX86 { get; set; }

	[JsonProperty("sign-dlls")]
	public bool SignDlls { get; set; }

	[JsonProperty("create-win-zip-package")]
	public bool CreateWinZipPackage { get; set; }

	[JsonProperty("create-system-zip-package")]
	public bool CreateSystemZipPackage { get; set; }

	[DefaultValue(new string[]{"http://sha256timestamp.ws.symantec.com/sha256/timestamp"})]
	[JsonProperty("timestamp-server-urls", DefaultValueHandling = DefaultValueHandling.Populate)]
	public string[] TimestampServerUrls { get; set; }

	[DefaultValue(@"C:\AureaCertificate\aurea-crm-cs-SHA-1.pfx")]
	[JsonProperty("certificate-path", DefaultValueHandling = DefaultValueHandling.Populate)]
	public string CertificatePath { get; set; }

	public string ToJSON()
	{
		return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
	}

	public void Log()
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(this.ToJSON());
		Console.ResetColor();
	}
}
