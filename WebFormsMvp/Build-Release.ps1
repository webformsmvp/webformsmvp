param (
	[Parameter(Mandatory=$true)]
	$ReleaseVersionNumber
)

# Ensure we have a clean workspace to start with
$tfStatus = & tf status
if ($tfStatus -ne "There are no pending changes.")
{
	throw "There are pending changes in your TFS workspace. Either undo, shelve or check in these changes prior to building the release."
}

# Confirm that there isn't an existing label that uses the version number

$solutionInfoPath = "SolutionInfo.cs"

# Checkout SolutionInfo.cs for edit
$tfCheckout = & tf checkout $solutionInfoPath
if ($tfCheckout -ne $solutionInfoPath)
{
	throw "There was a problem checking out SolutionInfo.cs"
}

# Set the version number in SolutionInfo.cs
(gc -Path $solutionInfoPath) `
	-replace "(?<=Version\(`")[.\d]*(?=`"\))", $ReleaseVersionNumber |
	sc -Path $solutionInfoPath -Encoding UTF8

# Build the solution in release mode

# Run the unit tests?

# Copy to a temp folder

# Remove the source bindings

# Package Releases\WebFormsMvp.(version).Library.zip (dll + pdb + xml)

# Package Releases\WebFormsMvp.(version).FeatureDemos.zip (feature demo w/ library reference)

# Check in SolutionInfo.cs

# Label the source tree for this version

# DEBUG ONLY
& tf undo $solutionInfoPath