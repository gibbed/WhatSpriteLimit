# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/WhatSpriteLimit/*" -Force -Recurse
dotnet publish "./WhatSpriteLimit.csproj" -c Release -o "$env:RELOADEDIIMODS/WhatSpriteLimit" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location