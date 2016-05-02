gci NuGet.Packages\*.symbols.nupkg | foreach-object { 
    tools\NuGet\NuGet.exe push $_ -Verbosity detailed -ApiKey ""
}

Write-Host "Press 'Enter' to continue ..."; Read-Host
