@echo off
echo Скачиваю правильный install.bat...
powershell -Command "Invoke-WebRequest -Uri 'https://pastebin.com/raw/zqJxW8Pp' -OutFile 'install.bat'"
echo Готово! Запустите install.bat от администратора.
pause
