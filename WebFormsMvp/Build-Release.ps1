param (
	[Parameter(Mandatory=$true)]
	[ValidatePattern("\d\.\d\.\d\.\d")]
	[string]
	$ReleaseVersionNumber
)

$PSScriptFilePath = (Get-Item $MyInvocation.MyCommand.Path).FullName

$SolutionRoot = Split-Path -Path $PSScriptFilePath -Parent

$Is64BitSystem = (Get-WmiObject -Class Win32_OperatingSystem).OsArchitecture -eq "64-bit"
$Is64BitProcess = [IntPtr]::Size -eq 8

$RegistryArchitecturePath = ""
if ($Is64BitProcess) { $RegistryArchitecturePath = "\Wow6432Node" }

$FrameworkArchitecturePath = ""
if ($Is64BitSystem) { $FrameworkArchitecturePath = "64" }

$ClrVersion = (Get-ItemProperty -path "HKLM:\SOFTWARE$RegistryArchitecturePath\Microsoft\VisualStudio\10.0")."CLR Version"

$MSBuild = "$Env:SYSTEMROOT\Microsoft.NET\Framework$FrameworkArchitecturePath\$ClrVersion\MSBuild.exe"

# Make sure we don't have a release folder for this version already
$ReleaseFolder = Join-Path -Path $SolutionRoot -ChildPath "Releases\v$ReleaseVersionNumber";
if ((Get-Item $ReleaseFolder -ErrorAction SilentlyContinue) -ne $null)
{
	Write-Warning "$ReleaseFolder already exists on your local machine. It will now be deleted."
	Remove-Item $ReleaseFolder -Recurse
}

# Set the version number in SolutionInfo.cs
$SolutionInfoPath = Join-Path -Path $SolutionRoot -ChildPath "SolutionInfo.cs"
(gc -Path $SolutionInfoPath) `
	-replace "(?<=Version\(`")[.\d]*(?=`"\))", $ReleaseVersionNumber |
	sc -Path $SolutionInfoPath -Encoding UTF8

# Build the solution in release mode
$SolutionPath = Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp.sln"
& $MSBuild "$SolutionPath" /p:Configuration=Release /maxcpucount /t:Clean
if (-not $?)
{
	throw "The MSBuild process returned an error code."
}
& $MSBuild "$SolutionPath" /p:Configuration=Release /maxcpucount
if (-not $?)
{
	throw "The MSBuild process returned an error code."
}

# Run the unit tests?

# Create a temp folder for the release assets
$LibraryReleaseFolder = Join-Path -Path $ReleaseFolder -ChildPath "Library";
$IntegrationsReleaseFolder = Join-Path -Path $LibraryReleaseFolder -ChildPath "Integrations";
$HelpersReleaseFolder = Join-Path -Path $LibraryReleaseFolder -ChildPath "Helpers";
New-Item $LibraryReleaseFolder -Type directory
New-Item $IntegrationsReleaseFolder -Type directory
New-Item $HelpersReleaseFolder -Type directory

# Add GettingStarted.txt to the temp folder
Set-Content (Join-Path -Path $LibraryReleaseFolder -ChildPath "GettingStarted.txt") -Value `
"This ZIP file only contains the compiled libraries:

  WebFormsMvp.dll  is the main library.

  Helpers/         contains other libraries that help with
                   development, but aren't actually used in
                   your application.

  Integrations/    contains useful adapters to connect
                   WebFormsMvp with other projects like Castle.

For help getting started, check out http://webformsmvp.com

There's also a feature demo app which you can browse at
http://webformsmvp.codeplex.com/SourceControl/BrowseLatest"

# Copy core library assets to temp folder (dll + pdb + xml)
$LibraryBinFolder = Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp\bin\Release"
Copy-Item "$LibraryBinFolder\*.*" -Destination $LibraryReleaseFolder -Include "WebFormsMvp.dll","WebFormsMvp.pdb","WebFormsMvp.xml"

# Copy Autofac assets to temp folder (dll + pdb + xml)
$AutofacBinFolder = Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp.Autofac\bin\Release"
Copy-Item "$AutofacBinFolder\*.*" -Destination $IntegrationsReleaseFolder -Include "WebFormsMvp.Autofac.dll","WebFormsMvp.Autofac.pdb","WebFormsMvp.Autofac.xml"

# Copy Castle assets to temp folder (dll + pdb + xml)
$CastleBinFolder = Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp.Castle\bin\Release"
Copy-Item "$CastleBinFolder\*.*" -Destination $IntegrationsReleaseFolder -Include "WebFormsMvp.Castle.dll","WebFormsMvp.Castle.pdb","WebFormsMvp.Castle.xml"

# Copy Unity assets to temp folder (dll + pdb + xml)
$UnityBinFolder = Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp.Unity\bin\Release"
Copy-Item "$UnityBinFolder\*.*" -Destination $IntegrationsReleaseFolder -Include "WebFormsMvp.Unity.dll","WebFormsMvp.Unity.pdb","WebFormsMvp.Unity.xml"

# Copy VS2010 CA rules to temp folder (dll + pdb)
$CodeAnalysisRulesBinFolder = Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp.CodeAnalysisRules\bin\Release"
Copy-Item "$CodeAnalysisRulesBinFolder\WebFormsMvp.CodeAnalysisRules.dll" `
	-Destination (Join-Path -Path $HelpersReleaseFolder -ChildPath "WebFormsMvp.CodeAnalysisRules.VS2010.dll")
Copy-Item "$CodeAnalysisRulesBinFolder\WebFormsMvp.CodeAnalysisRules.pdb" `
	-Destination (Join-Path -Path $HelpersReleaseFolder -ChildPath "WebFormsMvp.CodeAnalysisRules.VS2010.pdb")

# Build the CA rules again for FxCop 1.36 (different SDK + runtime)
$CodeAnalysisRulesProjectPath = Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp.CodeAnalysisRules\WebFormsMvp.CodeAnalysisRules.csproj"
$FxCop136DependencyPath = Join-Path -Path $SolutionRoot -ChildPath "Dependencies\FxCop136\"
& $MSBuild "$CodeAnalysisRulesProjectPath" /p:Configuration=Release /maxcpucount /t:Clean
if (-not $?)
{
	throw "The MSBuild process returned an error code."
}
& $MSBuild "$CodeAnalysisRulesProjectPath" /p:Configuration=Release /maxcpucount /p:CodeAnalysisPath="$FxCop136DependencyPath" /p:DefineConstants="" /p:TargetFrameworkVersion="v3.5"
if (-not $?)
{
	throw "The MSBuild process returned an error code."
}

# Copy FxCop 1.36 CA rules to temp folder (dll + pdb)
$CodeAnalysisRulesBinFolder = Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp.CodeAnalysisRules\bin\Release"
Copy-Item "$CodeAnalysisRulesBinFolder\WebFormsMvp.CodeAnalysisRules.dll" `
	-Destination (Join-Path -Path $HelpersReleaseFolder -ChildPath "WebFormsMvp.CodeAnalysisRules.FxCop136.dll")
Copy-Item "$CodeAnalysisRulesBinFolder\WebFormsMvp.CodeAnalysisRules.pdb" `
	-Destination (Join-Path -Path $HelpersReleaseFolder -ChildPath "WebFormsMvp.CodeAnalysisRules.FxCop136.pdb")

# Build the ZIP release
$LibraryReleaseZip = Join-Path -Path $ReleaseFolder -ChildPath "WebFormsMvp-v$ReleaseVersionNumber-Library.zip";
Add-Type -Path (Join-Path -Path $SolutionRoot -ChildPath "Dependencies\ICSharpCode.SharpZipLib.dll")
$FastZip = New-Object -TypeName ICSharpCode.SharpZipLib.Zip.FastZip
$FastZip.CreateZip($LibraryReleaseZip, $LibraryReleaseFolder, $true, "") # zipfile, source, recurse, filter

# Build the core NuGet package
$CoreNuSpecPath = Join-Path -Path $ReleaseFolder -ChildPath "WebFormsMvp.nuspec"
Copy-Item (Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp\WebFormsMvp.nuspec") `
	-Destination $CoreNuSpecPath
$NuGet = Join-Path -Path $SolutionRoot -ChildPath "Dependencies\NuGet.exe"
& $NuGet pack $CoreNuSpecPath -OutputDirectory $ReleaseFolder -Version $ReleaseVersionNumber
if (-not $?)
{
	throw "The NuGet process returned an error code."
}
Remove-Item $CoreNuSpecPath

# Build the Castle NuGet package
$CastleNuSpecPath = Join-Path -Path $ReleaseFolder -ChildPath "WebFormsMvp.Castle.nuspec"
(gc -Path (Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp.Castle\WebFormsMvp.Castle.nuspec")) `
	-replace "(?<=dependency id=`"webformsmvp`" version=`")[.\d]*(?=`")", $ReleaseVersionNumber |
	sc -Path $CastleNuSpecPath -Encoding UTF8
$NuGet = Join-Path -Path $SolutionRoot -ChildPath "Dependencies\NuGet.exe"
& $NuGet pack $CastleNuSpecPath -OutputDirectory $ReleaseFolder -Version $ReleaseVersionNumber
if (-not $?)
{
	throw "The NuGet process returned an error code."
}
Remove-Item $CastleNuSpecPath

# Tell the user what to do next
""
""
"If you're happy with this release build, you should now run the following commands:"
"    hg com -m `"Cutting release $ReleaseVersionNumber.`" SolutionInfo.cs"
"    hg tag -m `"Tagged version $ReleaseVersionNumber.`" v$ReleaseVersionNumber"
"    hg pus"
"    nuget push $CoreNuSpecPath"
"    nuget push $CastleNuSpecPath"