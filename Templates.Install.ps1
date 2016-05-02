function DoIt([string]$itemTemplateLocation, [string]$projectTemplateLocation)
{
    Write-Host $itemTemplateLocation 
    Write-Host $projectTemplateLocation
	Write-Host $group

    if ((Test-Path ($itemTemplateLocation + $group)) -ne $true)
    {
        New-Item -ItemType directory -Path ($itemTemplateLocation + $group)
    }

    gci "ItemTemplates\*.zip" | foreach-object { 
        Write-Host $_
        Copy-Item $_ ($itemTemplateLocation + $group) -Force
    }

    if ((Test-Path ($projectTemplateLocation + $group)) -ne $true)
    {
        New-Item -ItemType directory -Path ($projectTemplateLocation + $group)
    }

    gci "ProjectTemplates\*.zip" | foreach-object { 
        Write-Host $_
        Copy-Item $_ ($projectTemplateLocation + $group) -Force
    }
}


$group = "\Visual C#\SimControl"

if (Get-Item -Path Registry::"HKCU\Software\Microsoft\VisualStudio\10.0" -ErrorAction SilentlyContinue)
{
    $itemTemplateLocation = (Get-Item -Path Registry::"HKCU\Software\Microsoft\VisualStudio\10.0" | get-ItemProperty -Name UserItemTemplatesLocation).UserItemTemplatesLocation
    $projectTemplateLocation = (Get-Item -Path Registry::"HKCU\Software\Microsoft\VisualStudio\10.0" | get-ItemProperty -Name UserProjectTemplatesLocation).UserProjectTemplatesLocation

	DoIt $itemTemplateLocation $projectTemplateLocation
}

if (Get-Item -Path Registry::"HKCU\Software\Microsoft\VisualStudio\12.0" -ErrorAction SilentlyContinue)
{
    $itemTemplateLocation = (Get-Item -Path Registry::"HKCU\Software\Microsoft\VisualStudio\12.0" | get-ItemProperty -Name UserItemTemplatesLocation).UserItemTemplatesLocation
    $projectTemplateLocation = (Get-Item -Path Registry::"HKCU\Software\Microsoft\VisualStudio\12.0" | get-ItemProperty -Name UserProjectTemplatesLocation).UserProjectTemplatesLocation

	DoIt $itemTemplateLocation $projectTemplateLocation
}

if (Get-Item -Path Registry::"HKCU\Software\Microsoft\VisualStudio\12.0" -ErrorAction SilentlyContinue)
{
    $itemTemplateLocation = (Get-Item -Path Registry::"HKCU\Software\Microsoft\VisualStudio\14.0" | get-ItemProperty -Name UserItemTemplatesLocation).UserItemTemplatesLocation
    $projectTemplateLocation = (Get-Item -Path Registry::"HKCU\Software\Microsoft\VisualStudio\14.0" | get-ItemProperty -Name UserProjectTemplatesLocation).UserProjectTemplatesLocation

	DoIt $itemTemplateLocation $projectTemplateLocation
}

Write-Host "Press 'Enter' to continue ..."; Read-Host
