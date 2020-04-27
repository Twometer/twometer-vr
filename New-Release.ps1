param (
    [Parameter(Mandatory=$true)][string] $ReleaseVersion
)

$CurrentPath = (Get-Location).Path

$DriverReleasePath = "$CurrentPath\TVR.Driver\drivers"
$ServiceReleasePath = "$CurrentPath\TVR.Service\TVR.Service.CLI\bin\Release"
$WizardReleasePath = "$CurrentPath\TVR.Service\TVR.Service.Wizard\bin\Release"

$ReleasePath = "$CurrentPath\releases\rel-$ReleaseVersion"

# Create release folders
echo "Creating release folders..."
New-Item -Path $ReleasePath -ItemType Directory | Out-Null
New-Item -Path "$ReleasePath\driver\" -ItemType Directory | Out-Null
New-Item -Path "$ReleasePath\service\" -ItemType Directory | Out-Null
New-Item -Path "$ReleasePath\wizard\" -ItemType Directory | Out-Null

# Copy everything to the target directory
echo "Copying files..."
Copy-Item -Path "$DriverReleasePath\*" -Destination "$ReleasePath\driver\" -Recurse -Force
Copy-Item -Path "$ServiceReleasePath\*" -Destination "$ReleasePath\service\" -Recurse -Force
Copy-Item -Path "$WizardReleasePath\*" -Destination "$ReleasePath\wizard\" -Recurse -Force

# Removing debug files
echo "Removing debug files..."
Get-ChildItem "$ReleasePath\*.pdb" -Recurse | foreach { Remove-Item -Path $_.FullName }        # Debug databases
Get-ChildItem "$ReleasePath\.gitignore" -Recurse | foreach { Remove-Item -Path $_.FullName }   # Repository meta
Get-ChildItem "$ReleasePath\*.xml" -Recurse | foreach { Remove-Item -Path $_.FullName }        # Docs XML

# Compress runtime and wizard into zip archive
echo "Creating zip files..."
Compress-Archive -Path "$ReleasePath\driver\" -DestinationPath "$ReleasePath\tvr-runtime.zip" -Update
Compress-Archive -Path "$ReleasePath\service\" -DestinationPath "$ReleasePath\tvr-runtime.zip" -Update
Compress-Archive -Path "$ReleasePath\wizard\*" -DestinationPath "$ReleasePath\installer-tvr-$ReleaseVersion.zip"


# Remove temporary folders
echo "Cleaning up..."
Remove-Item -Path "$ReleasePath\driver\" -Recurse -Force
Remove-Item -Path "$ReleasePath\service\" -Recurse -Force
Remove-Item -Path "$Releasepath\wizard\" -Recurse -Force