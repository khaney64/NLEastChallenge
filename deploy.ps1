# Deploys NLEastChallenge to production (App Service NLEastChallenge; there is no staging site).
# Builds the current checkout, so run it from an up-to-date main.
#
# Usage:  .\deploy.ps1                 publish, zip and deploy
#         .\deploy.ps1 -PackageOnly    publish and zip only (dry run)
#
# If the execution policy blocks it: pwsh -ExecutionPolicy Bypass -File .\deploy.ps1
#
# Prereqs: .NET SDK matching the target framework, Node 24, Azure CLI, and a login to the app's tenant:
#   az login --tenant 55117e3a-e992-480d-8f42-095cd729530a
#
# See docs/security-remediation.md, items 1 and 5.

param([switch]$PackageOnly)

$ErrorActionPreference = 'Stop'
$Subscription = '3d6572f6-2e54-4530-a484-2847d25e01fa'
$ResourceGroup = 'NLEastRG'
$App = 'NLEastChallenge'
$Out = Join-Path $env:TEMP 'nleast-publish'
$Zip = Join-Path $env:TEMP 'nleast-publish.zip'

Set-Location $PSScriptRoot

$tfm = ([xml](Get-Content 'NLEastChallenge/NLEastChallenge.csproj')).Project.PropertyGroup.TargetFramework | Where-Object { $_ } | Select-Object -First 1
if ($tfm -notmatch '^net(\d+\.\d+)$') { throw "Unexpected TargetFramework '$tfm'" }
$expectedRuntime = "DOTNETCORE|$($Matches[1])"

if (-not $PackageOnly) {
    az account show --subscription $Subscription -o none 2>$null
    if ($LASTEXITCODE -ne 0) { throw "Not logged in to the NLEastChallenge subscription. Run: az login --tenant 55117e3a-e992-480d-8f42-095cd729530a" }

    # The net build only starts on the matching runtime image; deploying across a mismatch takes the site down.
    $runtime = az webapp config show -g $ResourceGroup -n $App --subscription $Subscription --query linuxFxVersion -o tsv
    if ($runtime -ne $expectedRuntime) {
        throw "App Service runtime is '$runtime' but the project targets $tfm (needs '$expectedRuntime'). Switch the runtime as part of the deploy; see docs/security-remediation.md item 5."
    }
}

Remove-Item $Out -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item $Zip -Force -ErrorAction SilentlyContinue

dotnet publish NLEastChallenge -c Release -r linux-x64 --self-contained false -o $Out
if ($LASTEXITCODE -ne 0) { throw 'dotnet publish failed' }

# tar writes forward-slash entry names, which the Linux App Service needs.
Push-Location $Out
try {
    tar -a -c -f $Zip *
    if ($LASTEXITCODE -ne 0) { throw 'zip failed' }
}
finally { Pop-Location }
Write-Host "Package: $Zip ($tfm, expects $expectedRuntime)"

if ($PackageOnly) { return }

# --clean true removes files from the previous build (old assets would otherwise stay downloadable).
az webapp deploy -g $ResourceGroup -n $App --subscription $Subscription --type zip --clean true --src-path $Zip
if ($LASTEXITCODE -ne 0) { throw 'az webapp deploy failed' }

Write-Host 'Deployed: https://nleast-challenge.com'
