[Reflection.Assembly]::LoadWithPartialName( "System.IO.Compression.FileSystem" )

function RemoveItems($items)
{
    foreach ($i in $items)
    {
        if ((Test-Path $i) -eq $true)
        {
            Remove-Item ($i) -Force -Recurse
        }
    }
}   

gci *\*.vstemplate | foreach-object { 
    $t = $_.BaseName

    $temp =  (Resolve-Path "ProjectTemplates").Path + "\" + $t
    Write-Host $temp

    RemoveItems $temp, ("ProjectTemplates\"+$t+".zip")
    
    Copy-Item -Path $t -Destination "ProjectTemplates" -Recurse
    RemoveItems ($temp+"\bin"), ($temp+"\obj"), ($temp+"\"+$t+".csproj.user")
    Get-ChildItem $temp -Recurse -include "*.cs", "*.csproj" | 
        foreach-object {  $a = $_.fullname; (get-content $a) | 
            foreach-object { $_ -replace $t, '$safeprojectname$' } |
                set-content $a 
        }

    [System.IO.Compression.ZipFile]::CreateFromDirectory($temp, $temp+".zip", [System.IO.Compression.CompressionLevel]::Optimal, $false )

    RemoveItems $temp
}

Write-Host "Press 'Enter' to continue ..."; Read-Host