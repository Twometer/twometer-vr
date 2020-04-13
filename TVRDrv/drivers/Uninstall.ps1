function Get-SteamVrPath() {
	$JSON = Get-Content "$env:LOCALAPPDATA\openvr\openvrpaths.vrpath" | Out-String | ConvertFrom-Json
    if (!$JSON) {
        return $null;
    }

    foreach ($path in $JSON.runtime) {
        $registryPath = "$path/bin/win64/vrpathreg.exe"
        if (Test-Path $registryPath) {
            return $path;
        }
    }

    return $null;
}

function Unregister-Driver($steamVrPath, $installDir) {
    $registryCmd = "$steamVrPath\bin\win64\vrpathreg.exe"
    & cmd /c """$registryCmd"" removedriver ""$installDir"""
}

$steamVrPath = Get-SteamVrPath

if ($steamVrPath -eq $null) {
    Write-Error "SteamVR is not installed on this system"
    exit -1
}
Write-Output "Found SteamVR installation, uninstalling the driver"

# Unregister driver
$targetDir = (Get-Location).Path + '\tvr'
Unregister-Driver $steamVrPath $targetDir