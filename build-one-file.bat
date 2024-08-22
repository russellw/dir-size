dotnet publish /p:Configuration=Release /p:Platform="Any CPU" -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:PublishTrimmed=true -p:TrimMode=link -p:ReadyToRun=true -p:DebugType=None
move bin\Release\net7.0\win-x64\publish\*.exe
