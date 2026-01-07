# Makefile для TESTBASH
# Автор: [Ваше ФИО]

# Переменные
APP_NAME = TESTBASH
VERSION = 1.0.0
CSC = mcs
CSC_FLAGS = -target:winexe -out:$(APP_NAME).exe -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:System.Data.dll
SRC_DIR = Source
BIN_DIR = bin
DOCS_DIR = docs
CONF_DIR = etc
LOG_DIR = logs
TMP_DIR = tmp
INSTALL_DIR = /usr/local/$(APP_NAME)
BIN_INSTALL_DIR = /usr/bin
CERT_DIR = /usr/etc/$(APP_NAME)

# Цель по умолчанию
all: compile docs

# ============ КОМПИЛЯЦИЯ ============
compile: $(BIN_DIR)/$(APP_NAME).exe

$(BIN_DIR)/$(APP_NAME).exe: $(SRC_DIR)/**/*.cs
	@echo "Компиляция проекта..."
	@mkdir -p $(BIN_DIR)
	$(CSC) $(CSC_FLAGS) $(SRC_DIR)/**/*.cs
	@mv $(APP_NAME).exe $(BIN_DIR)/
	@cp run.sh $(BIN_DIR)/
	@chmod +x $(BIN_DIR)/run.sh
	@echo "✓ Компиляция завершена"

# ============ ДОКУМЕНТАЦИЯ ============
user_man:
	@echo "Создание руководства пользователя..."
	@mkdir -p $(DOCS_DIR)
	@cp AUTHOR $(DOCS_DIR)/
	@cp README $(DOCS_DIR)/
	@echo "✓ Руководство пользователя создано"

in_instruct:
	@echo "Создание инструкции по установке..."
	@echo "Инструкция по установке TESTBASH" > $(DOCS_DIR)/INSTALL
	@echo "===============================" >> $(DOCS_DIR)/INSTALL
	@echo "" >> $(DOCS_DIR)/INSTALL
	@echo "1. Установите зависимости:" >> $(DOCS_DIR)/INSTALL
	@echo "   sudo apt install mono-devel" >> $(DOCS_DIR)/INSTALL
	@echo "" >> $(DOCS_DIR)/INSTALL
	@echo "2. Скомпилируйте программу:" >> $(DOCS_DIR)/INSTALL
	@echo "   make compile" >> $(DOCS_DIR)/INSTALL
	@echo "" >> $(DOCS_DIR)/INSTALL
	@echo "3. Настройте систему:" >> $(DOCS_DIR)/INSTALL
	@echo "   make configure" >> $(DOCS_DIR)/INSTALL
	@echo "" >> $(DOCS_DIR)/INSTALL
	@echo "4. Установите программу:" >> $(DOCS_DIR)/INSTALL
	@echo "   sudo make install" >> $(DOCS_DIR)/INSTALL
	@echo "✓ Инструкция по установке создана"

license:
	@echo "Создание лицензии..."
	@echo "MIT License" > $(DOCS_DIR)/LICENSE
	@echo "" >> $(DOCS_DIR)/LICENSE
	@echo "Copyright (c) $(shell date +%Y) [Ваше ФИО]" >> $(DOCS_DIR)/LICENSE
	@echo "" >> $(DOCS_DIR)/LICENSE
	@cat >> $(DOCS_DIR)/LICENSE << EOF
Данная лицензия разрешает лицам, получившим копию данного программного
обеспечения и сопутствующей документации, безвозмездно использовать
Программное обеспечение без ограничений.

ПРОГРАММНОЕ ОБЕСПЕЧЕНИЕ ПРЕДОСТАВЛЯЕТСЯ «КАК ЕСТЬ», БЕЗ КАКИХ-ЛИБО ГАРАНТИЙ.
EOF
	@echo "✓ Лицензия создана"

docs: user_man in_instruct license
	@echo "✓ Вся документация собрана"

# ============ НАСТРОЙКА ============
configure: $(CONF_DIR)/install.conf

$(CONF_DIR)/install.conf:
	@echo "Настройка системы..."
	@mkdir -p $(CONF_DIR)
	@read -p "Введите ваше ФИО: " fullname; \
	read -p "Введите ваш email: " email; \
	while [[ ! "$$email" =~ ^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$$ ]]; do \
		echo "Некорректный email!"; \
		read -p "Введите ваш email: " email; \
	done; \
	read -p "Введите ваш телефон: " phone; \
	while [[ ! "$$phone" =~ ^[0-9+ -]{10,}$$ ]]; do \
		echo "Некорректный телефон!"; \
		read -p "Введите ваш телефон: " phone; \
	done; \
	echo "Создание сертификата..." > $(CONF_DIR)/certificate.conf; \
	echo "FULL_NAME=$$fullname" >> $(CONF_DIR)/certificate.conf; \
	echo "EMAIL=$$email" >> $(CONF_DIR)/certificate.conf; \
	echo "PHONE=$$phone" >> $(CONF_DIR)/certificate.conf; \
	echo "DATE=$$(date '+%Y-%m-%d %H:%M:%S')" >> $(CONF_DIR)/certificate.conf; \
	echo "LICENSE=MIT" >> $(CONF_DIR)/certificate.conf; \
	echo "✓ Сертификат создан"; \
	echo "Настройка путей..."; \
	read -p "Путь установки [$(INSTALL_DIR)]: " install_path; \
	install_path=$${install_path:-$(INSTALL_DIR)}; \
	read -p "Путь для временных файлов [$(TMP_DIR)]: " temp_path; \
	temp_path=$${temp_path:-$(TMP_DIR)}; \
	read -p "Путь для логов [$(LOG_DIR)]: " log_path; \
	log_path=$${log_path:-$(LOG_DIR)}; \
	read -p "Путь для сохранений [$$HOME/.$(APP_NAME)/saves]: " save_path; \
	save_path=$${save_path:-$$HOME/.$(APP_NAME)/saves}; \
	echo "INSTALL_PATH=$$install_path" > $(CONF_DIR)/install.conf; \
	echo "TEMP_PATH=$$temp_path" >> $(CONF_DIR)/install.conf; \
	echo "LOG_DIR=$$log_path" >> $(CONF_DIR)/install.conf; \
	echo "SAVE_PATH=$$save_path" >> $(CONF_DIR)/install.conf; \
	echo "DOCS_PATH=/usr/local/share/doc/$(APP_NAME)" >> $(CONF_DIR)/install.conf; \
	echo "✓ Настройка завершена"

# ============ УСТАНОВКА ============
install: compile docs configure
	@echo "Установка системы..."
	@if [ ! -f $(CONF_DIR)/install.conf ]; then \
		echo "Ошибка: сначала выполните настройку (make configure)"; \
		exit 1; \
	fi
	@if [ ! -f $(CONF_DIR)/certificate.conf ]; then \
		echo "Ошибка: сертификат не найден"; \
		exit 1; \
	fi
	@echo "Проверка сертификата..."
	@. $(CONF_DIR)/certificate.conf; \
	cert_date=$$(echo "$$DATE" | sed 's/ /_/g'); \
	cert_time=$$(date -d "$$cert_date" +%s 2>/dev/null || date +%s); \
	current_time=$$(date +%s); \
	if [ $$((current_time - cert_time)) -gt 86400 ]; then \
		echo "Предупреждение: сертификат старше суток"; \
	fi
	@echo "Создание директорий..."
	@. $(CONF_DIR)/install.conf; \
	mkdir -p $$INSTALL_PATH; \
	mkdir -p $$TEMP_PATH; \
	mkdir -p $$LOG_DIR; \
	mkdir -p $$SAVE_PATH; \
	mkdir -p $(CERT_DIR); \
	mkdir -p /etc/$(APP_NAME); \
	mkdir -p /var/log/$(APP_NAME); \
	mkdir -p /usr/local/share/doc/$(APP_NAME)
	@echo "Копирование файлов..."
	@cp $(BIN_DIR)/$(APP_NAME).exe $$INSTALL_PATH/
	@cp $(BIN_DIR)/run.sh $$INSTALL_PATH/
	@cp -r $(DOCS_DIR)/* /usr/local/share/doc/$(APP_NAME)/
	@cp $(CONF_DIR)/install.conf /etc/$(APP_NAME)/
	@cp $(CONF_DIR)/certificate.conf $(CERT_DIR)/certificate.crt
	@echo "Настройка прав..."
	@chmod 755 $$INSTALL_PATH/$(APP_NAME).exe
	@chmod 755 $$INSTALL_PATH/run.sh
	@chmod 644 $(CERT_DIR)/certificate.crt
	@echo "Создание символических ссылок..."
	@ln -sf $$INSTALL_PATH/$(APP_NAME).exe $(BIN_INSTALL_DIR)/$(APP_NAME)
	@ln -sf $$SAVE_PATH $$INSTALL_PATH/saves
	@ln -sf $$LOG_DIR $$INSTALL_PATH/logs
	@echo "✓ Установка завершена"
	@echo ""
	@echo "ИНФОРМАЦИЯ:"
	@echo "  Программа установлена в: $$INSTALL_PATH"
	@echo "  Исполняемый файл: $(BIN_INSTALL_DIR)/$(APP_NAME)"
	@echo "  Документация: /usr/local/share/doc/$(APP_NAME)"
	@echo "  Сертификат: $(CERT_DIR)/certificate.crt"

# ============ ОЧИСТКА ============
clear_tmp:
	@echo "Очистка временных файлов..."
	@rm -rf $(TMP_DIR)/*
	@echo "✓ Временные файлы очищены"

clear_config:
	@echo "Удаление конфигурационных файлов..."
	@rm -rf $(CONF_DIR)
	@echo "✓ Конфигурационные файлы удалены"

clear_compile: clear_tmp
	@echo "Удаление скомпилированных файлов..."
	@rm -rf $(BIN_DIR)
	@echo "✓ Скомпилированные файлы удалены"

clear: clear_tmp clear_config

# ============ УДАЛЕНИЕ ============
uninstall:
	@echo "Удаление системы..."
	@if [ -f $(CONF_DIR)/install.conf ]; then \
		. $(CONF_DIR)/install.conf; \
		rm -rf $$INSTALL_PATH; \
		rm -rf $$TEMP_PATH; \
	fi
	@rm -rf /etc/$(APP_NAME)
	@rm -rf $(CERT_DIR)
	@rm -f $(BIN_INSTALL_DIR)/$(APP_NAME)
	@rm -rf /var/log/$(APP_NAME)
	@echo "✓ Система удалена"
	@echo "Примечание: файлы сохранений не удалены"

remove: uninstall
	@echo "Удаление логов..."
	@rm -rf $(LOG_DIR)
	@rm -rf /var/log/$(APP_NAME)/*.log
	@echo "✓ Логи удалены"

remove_all: remove
	@echo "Удаление сохранений..."
	@if [ -f $(CONF_DIR)/install.conf ]; then \
		. $(CONF_DIR)/install.conf; \
		rm -rf $$SAVE_PATH; \
	fi
	@echo "✓ Все файлы удалены"

# ============ ДРУГИЕ ЦЕЛИ ============
run: compile
	@./run.sh

test: compile
	@echo "Тестирование программы..."
	@./run.sh -h
	@./run.sh -v
	@echo "✓ Тестирование завершено"

clean: clear_compile clear_config
	@echo "✓ Очистка завершена"

distclean: remove_all clean
	@echo "✓ Полная очистка завершена"

help:
	@echo "Доступные команды:"
	@echo "  make compile     - скомпилировать проект"
	@echo "  make docs        - создать документацию"
	@echo "  make configure   - настроить систему"
	@echo "  make install     - установить программу"
	@echo "  make run         - запустить программу"
	@echo "  make test        - протестировать программу"
	@echo "  make clean       - очистить проект"
	@echo "  make uninstall   - удалить программу"
	@echo "  make help        - показать эту справку"

.PHONY: all compile docs configure install clear_tmp clear_config clear_compile clear uninstall remove remove_all run test clean distclean help user_man in_instruct license
