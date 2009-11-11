param (
	[Parameter(Mandatory=$true)]
	$ReleaseVersionNumber
)

$PSScriptFilePath = (get-item $MyInvocation.MyCommand.Path).FullName

$SolutionRoot = Split-Path -Path $PSScriptFilePath -Parent

$Is64BitSystem = (Get-WmiObject -Class Win32_OperatingSystem).OsArchitecture -eq "64-bit"
$Is64BitProcess = [IntPtr]::Size -eq 8

$RegistryArchitecturePath = ""
if ($Is64BitProcess) { $RegistryArchitecturePath = "\Wow6432Node" }
$ClrVersion = (Get-ItemProperty -path "HKLM:\SOFTWARE$RegistryArchitecturePath\Microsoft\VisualStudio\10.0")."CLR Version"
$VSInstallDir = (Get-ItemProperty -path "HKLM:\SOFTWARE$RegistryArchitecturePath\Microsoft\VisualStudio\10.0").InstallDir

$FrameworkArchitecturePath = ""
if ($Is64BitSystem) { $FrameworkArchitecturePath = "64" }
$MSBuild = "$Env:SYSTEMROOT\Microsoft.NET\Framework$FrameworkArchitecturePath\$ClrVersion\MSBuild.exe"

$TF = Join-Path -Path $VSInstallDir -ChildPath "tf.exe"

# Check for any pending changes
$PendingChangesPriorToBuild = (& $TF status "$SolutionRoot\*") -ne "There are no pending changes."
if ($PendingChangesPriorToBuild)
{
	Write-Warning "There are pending changes in your TFS workspace. This will be marked as an unsafe release and should not be uploaded to CodePlex."
}

# Confirm that there isn't an existing label that uses the version number

# Checkout SolutionInfo.cs
$SolutionInfoName = "SolutionInfo.cs"
$SolutionInfoPath = Join-Path -Path $SolutionRoot -ChildPath $SolutionInfoName
$SolutionInfoCheckoutResult = & $TF checkout "$SolutionInfoPath"
if ($SolutionInfoCheckoutResult -ne $SolutionInfoName)
{
	throw "There was a problem checking out SolutionInfo.cs. The command returned:\r\n\r\n$SolutionInfoCheckoutResult"
}

# Set the version number in SolutionInfo.cs
(gc -Path $SolutionInfoPath) `
	-replace "(?<=Version\(`")[.\d]*(?=`"\))", $ReleaseVersionNumber |
	sc -Path $SolutionInfoPath -Encoding UTF8

# Build the solution in release mode
$SolutionPath = Join-Path -Path $SolutionRoot -ChildPath "WebFormsMvp.sln"
& $MSBuild "$SolutionPath" /p:Configuration=Release

# Check the build result

# Run the unit tests?

# Copy to a temp folder

# Remove the source bindings

# Package Releases\WebFormsMvp.(version).Library.zip (dll + pdb + xml)

# Package Releases\WebFormsMvp.(version).FeatureDemos.zip (feature demo w/ library reference)

# Check in SolutionInfo.cs

# Label the source tree for this version

# DEBUG ONLY
& $TF undo $solutionInfoPath