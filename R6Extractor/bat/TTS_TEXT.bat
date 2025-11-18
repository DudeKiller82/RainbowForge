set R6PATH=E:\SteamLibrary\steamapps\common\Tom Clancy's Rainbow Six Siege - Test Server
set DESTFOLDER=TEXTTS
set SUBPATH=_textures
set OODLE2_8_PATH=.\oo2core_8_win64.dll

cd..
set /A FOLDERNUMBER=0
setlocal ENABLEDELAYEDEXPANSION
if exist "..\%DESTFOLDER%\" rmdir /S /Q "..\%DESTFOLDER%"
mkdir "..\%DESTFOLDER%"
mkdir "..\%DESTFOLDER%\Misc"
mkdir "..\%DESTFOLDER%\Diffuse"
for %%f in ("%R6PATH%\*%SUBPATH%*.forge") do (
	DumpTool.exe dumpall "%%f"
	for %%d in (*.dds) do (
		texconv.exe -vflip -ft png -srgbi -l -f R8G8B8A8_UNORM_SRGB "%%d"
	)
	mkdir "..\%DESTFOLDER%\Misc\!FOLDERNUMBER!"
	mkdir "..\%DESTFOLDER%\Diffuse\!FOLDERNUMBER!"
	move *Misc*.png "..\%DESTFOLDER%\Misc\!FOLDERNUMBER!"
	move *Diffuse*.png "..\%DESTFOLDER%\Diffuse\!FOLDERNUMBER!"
	del *.dds
	set /A FOLDERNUMBER+=1
)
endlocal
pause