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

function Register-Driver($steamVrPath, $installDir) {
    $registryCmd = "$steamVrPath\bin\win64\vrpathreg"
    & cmd /c """$registryCmd"" adddriver ""$installDir"""
}

$steamVrPath = Get-SteamVrPath

if ($steamVrPath -eq $null) {
    Write-Error "SteamVR is not installed on this system"
    exit -1
}
Write-Output "Found SteamVR installation, installing the driver"

# Register driver
$targetDir = (Get-Location).Path + '\tvr'
Register-Driver $steamVrPath $targetDir