$solution = "$([io.path]::GetFileName($PSScriptRoot)).sln"
$msbuildExe = join-path -path (Get-ItemProperty "HKLM:\software\Microsoft\MSBuild\ToolsVersions\14.0")."MSBuildToolsPath" -childpath "msbuild.exe"

&$msbuildExe /maxcpucount:2 /target:Rebuild /property:Configuration=Release /property:RunCodeAnalysis=false /verbosity:minimal $solution

if ($?) {
    Write-Host "Press 'Enter' to continue ..."; Read-Host

    gci *\*.nuspec | foreach-object { 
        $p = $_.BaseName
        tools\NuGet\NuGet.exe pack $p\$p.csproj -NonInteractive -OutputDirectory ..\SimControl.Reactive.NuGet -includereferencedprojects -symbols -Properties Configuration=Release
    }
}

Write-Host "Press 'Enter' to continue ..."; Read-Host