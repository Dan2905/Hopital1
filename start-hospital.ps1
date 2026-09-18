$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$webRoot = Join-Path $projectRoot "src\HospitalManagement.Web"
$dllDir = Join-Path $webRoot "bin\Debug\net10.0"
$dllPath = Join-Path $dllDir "HospitalManagement.Web.dll"

if (-not (Test-Path $dllPath)) {
    throw "DLL introuvable : $dllPath. Compilez d'abord le projet."
}

Write-Host "Démarrage de l'application Hospital Management..." -ForegroundColor Cyan
Write-Host "URL: http://localhost:5001" -ForegroundColor Yellow

# Démarrage direct du DLL avec le bon content root pour servir les fichiers statiques (CSS/JS).
dotnet $dllPath --urls "http://localhost:5001" --contentRoot $webRoot
