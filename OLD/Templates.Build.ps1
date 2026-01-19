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

function CreateTemplates($templateType)
{
    New-Item -ItemType Directory -Force -Path $templateType

    gci ("*"+$templateType+"*\*.vstemplate") | foreach-object { 
        $t = $_.BaseName

        $temp = (Resolve-Path $templateType).Path + "\" + $t
        Write-Host $t

        RemoveItems $temp, ($templateType+"\"+$t+".zip")
    
        Copy-Item -Path $t -Destination $templateType -Recurse
        RemoveItems ($temp+"\bin"), ($temp+"\obj"), ($temp+"\"+$t+".csproj.user")
        Get-ChildItem $temp -Recurse -include "*.cs", "*.csproj" | 
            foreach-object {  $a = $_.fullname; (get-content $a) | 
                foreach-object { $_ -replace $t, '$safeprojectname$' } |
                    set-content $a 
            }

        [System.IO.Compression.ZipFile]::CreateFromDirectory($temp, $temp+".zip", [System.IO.Compression.CompressionLevel]::Optimal, $false )

        RemoveItems $temp
    }
}


CreateTemplates "ItemTemplates"
CreateTemplates "ProjectTemplates"

Write-Host "Press 'Enter' to continue ..."; Read-Host
