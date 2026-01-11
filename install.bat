@echo off
chcp 1251
title Установка TESTBASH из GitHub
color 0A

echo ========================================
echo   УСТАНОВКА TESTBASH ИЗ GITHUB
echo ========================================
echo.
echo Репозиторий: https://github.com/sanspapsspy/TESTBASH
echo.

net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!] Запустите от имени администратора!
    pause
    exit /b 1
)

set "TEMP_DIR=%TEMP%\TESTBASH_%RANDOM%"
set "INSTALL_DIR=%ProgramFiles%\TESTBASH"
mkdir "%TEMP_DIR%"

echo Скачивание с GitHub...
powershell -Command "Invoke-WebRequest -Uri 'https://github.com/sanspapsspy/TESTBASH/archive/refs/heads/master.zip' -OutFile '%TEMP_DIR%\repo.zip'"

echo Распаковка...
powershell -Command "Expand-Archive -Path '%TEMP_DIR%\repo.zip' -DestinationPath '%TEMP_DIR%' -Force"

echo Поиск программы...
dir /s /b "%TEMP_DIR%\*.exe" > "%TEMP_DIR%\exes.txt"
set /p EXE_FILE=<"%TEMP_DIR%\exes.txt"

if not exist "%EXE_FILE%" (
    echo Ошибка: .exe файл не найден!
    echo Соберите проект в Visual Studio.
    pause
    exit /b 1
)

echo Копирование...
mkdir "%INSTALL_DIR%" 2>nul
for %%f in ("%EXE_FILE%") do set "EXE_DIR=%%~dpf"
xcopy "%EXE_DIR%*" "%INSTALL_DIR%\" /E /I /Y

echo Создание ярлыка...
set "DESKTOP=%USERPROFILE%\Desktop"
powershell -Command "$s=(New-Object -COM WScript.Shell).CreateShortcut('%DESKTOP%\TESTBASH.lnk');$s.TargetPath='%INSTALL_DIR%\TESTBASH.exe';$s.WorkingDirectory='%INSTALL_DIR%';$s.Save()"

rd /S /Q "%TEMP_DIR%"

echo.
echo УСТАНОВКА ЗАВЕРШЕНА!
echo.
echo Ярлык: %DESKTOP%\TESTBASH.lnk
echo Папка: %INSTALL_DIR%
echo.
pause
