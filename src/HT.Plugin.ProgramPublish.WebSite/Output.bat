@ echo off

md %2output\bin\"
echo %date% - %time% > %2\output\bin\remark.txt
md %2output\Plugins\%1"

copy bin\HT.Plugin.ProgramPublish.dll					%2\output\Plugins\%1
copy bin\HT.Plugin.ProgramPublish.Services.dll			%2\output\Plugins\%1
copy bin\HT.Plugin.ProgramPublish.WebSite.dll			%2\output\Plugins\%1
copy bin\ffmpeg.exe										%2\output\Plugins\%1
copy bin\Description.xml								%2\output\Plugins\%1

@ echo.