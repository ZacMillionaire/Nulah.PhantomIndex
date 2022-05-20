Param(
    [Parameter(Mandatory=$True,Position=0)][String]$projectDirectory,
    [Parameter(Mandatory=$True,Position=1)][String]$pluginTarget
)

#sample post build script for building plugins during development and copying to the WPF project

"Project Directory: $projectDirectory"
"Plugin Target: $pluginTarget"

Remove-Item -Path $pluginTarget -Force -Recurse -ErrorAction SilentlyContinue

Copy-Item -Path $projectDirectory -Destination $pluginTarget -Exclude @("Nulah.PhantomIndex.Core.*", "Nulah.PhantomIndex.Lib.*") -Recurse -Force