param(
  [string]$Action = "open"
)

$Unity   = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"
$Project = $PSScriptRoot
$Log     = "E:\tmp\unity-build.log"

if (-not (Test-Path "E:\tmp")) { New-Item -ItemType Directory -Path "E:\tmp" | Out-Null }

function Stop-UnityEditor {
  $procs = Get-Process -Name Unity -ErrorAction SilentlyContinue
  if ($procs) {
    Write-Host "Stopping existing Unity.exe ($($procs.Count) proc)"
    $procs | Stop-Process -Force
    Start-Sleep -Seconds 2
  }
  if (Test-Path "$Project\Temp\UnityLockfile") {
    Remove-Item "$Project\Temp\UnityLockfile" -Force -ErrorAction SilentlyContinue
  }
}

switch ($Action) {
  "compile" {
    Stop-UnityEditor
    Write-Host "Compiling: $Project"
    Remove-Item $Log -Force -ErrorAction SilentlyContinue
    & $Unity -projectPath $Project -batchmode -quit -logFile $Log
    if (Test-Path $Log) {
      $errors = Select-String -Path $Log -Pattern "error CS"
      if ($errors) {
        Write-Host "COMPILE ERRORS:"
        $errors | ForEach-Object { Write-Host $_.Line }
        exit 1
      }
    }
    Write-Host "OK - no errors"
  }
  default {
    Write-Host "Opening Unity Editor: $Project"
    Start-Process -FilePath $Unity -ArgumentList "-projectPath", "`"$Project`""
  }
}
