#!/bin/bash
# Скрипт запуска TESTBASH

APP_PATH="./bin/Debug/TESTBASH.exe"

if [ ! -f "$APP_PATH" ]; then
    echo "Ошибка: файл не найден. Скомпилируйте проект в Visual Studio"
    exit 1
fi

echo "Запуск TESTBASH..."
echo "=================="

# Проверка аргументов
if [ $# -eq 0 ]; then
    echo "Режим: интерактивный"
    mono "$APP_PATH"
elif [ "$1" == "-h" ] || [ "$1" == "--help" ]; then
    mono "$APP_PATH" -h
elif [ "$1" == "-m" ] || [ "$1" == "--menu" ]; then
    echo "Режим: графический интерфейс"
    mono "$APP_PATH" -m
elif [ "$1" == "-f" ] && [ -n "$2" ]; then
    echo "Обработка файла: $2"
    if [ -n "$3" ] && [ "$3" == "-o" ] && [ -n "$4" ]; then
        mono "$APP_PATH" -f "$2" -o "$4"
    else
        mono "$APP_PATH" -f "$2"
    fi
else
    echo "Аргументы: $@"
    mono "$APP_PATH" "$@"
fi
