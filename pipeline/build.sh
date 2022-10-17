#!/bin/bash

cd app/src

dotnet --info
dotnet --list-sdks

echo Clean started on 'date'
dotnet Clean

echo Restore started on 'date'
dotnet restore -r=linux-x64

echo Build started on 'date'
dotnet build --no-restore -c=Release -r=linux-x64

cd main/Template.Aws.Lambda.Api

echo Publish started on 'date'
dotnet publish --no-build -o=/builded/dlls -c=Release -r=linux-x64 \
  -p:PublishReadyToRun=true