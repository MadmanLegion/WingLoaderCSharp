set "zipfile=WingLoaderC#.zip"

REM Create the zip file (and overwrite if it already exists)
if exist "%zipfile%" del "%zipfile%"

if exist "Wing.zip" del "Wing.zip
if exist "WingAT.zip" del "WingAT.zip
if exist "WingKS.zip" del "WingKS.zip
if exist "Wing2.zip" del "Wing2.zip
if exist "Wing2AT.zip" del "Wing2AT.zip
if exist "Wing2KS.zip" del "Wing2KS.zip
if exist "FFMPEG" rmdir /s /q "FFMPEG"

copy "S:\P4WS\Source\WingLoader\Packages\Release Archive"\*.zip .

if exist "dosbox-staging-windows-x64-v0.83.0-RC1.zip" del "dosbox-staging-windows-x64-v0.83.0-RC1.zip"

REM Compress all files and subdirectories, excluding the batch script
powershell -Command "Get-ChildItem -Path . -Exclude 'ReleasePackage.bat', '%zipfile%' | Compress-Archive -DestinationPath '%zipfile%' -Force"
powershell -Command "Get-ChildItem -Path . -Exclude 'ReleasePackage.bat', '%zipfile%' | Remove-Item -Force -Recurse"
