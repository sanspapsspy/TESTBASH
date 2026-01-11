@echo off
title Создание установщика
echo Создаю install.bat с правильной кодировкой...

(
echo @echo off
echo chcp 1251 ^>nul
echo title Установка TESTBASH из GitHub
echo echo ========================================
echo echo    УСТАНОВКА TESTBASH ИЗ GITHUB
echo echo ========================================
echo echo.
echo echo Репозиторий: https://github.com/sanspapsspy/TESTBASH
echo echo.
echo net session ^>nul 2^>^&1
echo if errorlevel 1 ^(
echo    echo [!] ЗАПУСТИТЕ ОТ АДМИНИСТРАТОРА!
echo    echo.
echo    pause
echo    exit /b 1
echo ^)
echo.
echo echo Установка в процессе...
echo mkdir "C:\Program Files\TESTBASH" 2^>nul
echo echo Папка создана: C:\Program Files\TESTBASH
echo echo.
echo echo 1. Скачайте проект с GitHub
echo echo 2. Найдите TESTBASH.exe
echo echo 3. Скопируйте в папку выше
echo echo.
echo pause
) > "%USERPROFILE%\Desktop\install.bat"

echo Готово! Файл создан на рабочем столе: install.bat
echo Запустите его от имени администратора.
pause