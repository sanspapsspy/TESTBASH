using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TESTBASH
{
    public partial class MainForm : Form
    {
        // Элементы интерфейса
        private RichTextBox inputTextBox;
        private RichTextBox outputTextBox;
        private TextBox searchTextBox;
        private TextBox replaceTextBox;

        private Button loadButton;
        private Button saveButton;
        private Button processButton;
        private Button searchButton;
        private Button replaceButton;
        private Button clearButton;
        private Button statsButton;
        private Button formatButton;
        private Button copyButton;

        private Label inputLabel;
        private Label outputLabel;
        private Label searchLabel;
        private Label replaceLabel;
        private Label statsLabel;

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem editMenu;
        private ToolStripMenuItem toolsMenu;
        private ToolStripMenuItem helpMenu;

        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public MainForm()
        {
            InitializeComponents();
            SetupForm();
        }

        /// <summary>
        /// Инициализация компонентов формы
        /// </summary>
        private void InitializeComponents()
        {
            // Настройки окна
            this.Text = "TextProcessorVS - Обработка текста";
            this.Size = new System.Drawing.Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.Font = new System.Drawing.Font("Segoe UI", 9);

            // Создание элементов интерфейса
            CreateMenuStrip();
            CreateInputArea();
            CreateOutputArea();
            CreateControlButtons();
            CreateSearchReplaceArea();
            CreateStatsArea();

            // Создание диалоговых окон
            openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Выберите файл для загрузки",
                Multiselect = false
            };

            saveFileDialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Сохранить результат",
                DefaultExt = "txt"
            };
        }

        /// <summary>
        /// Создание меню
        /// </summary>
        private void CreateMenuStrip()
        {
            menuStrip = new MenuStrip();
            menuStrip.BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
            menuStrip.ForeColor = System.Drawing.Color.White;

            // Меню Файл
            fileMenu = new ToolStripMenuItem("Файл");
            var openMenuItem = new ToolStripMenuItem("Открыть", null, OpenFile_Click);
            openMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            var saveMenuItem = new ToolStripMenuItem("Сохранить", null, SaveFile_Click);
            saveMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            var exitMenuItem = new ToolStripMenuItem("Выход", null, Exit_Click);
            exitMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;

            fileMenu.DropDownItems.Add(openMenuItem);
            fileMenu.DropDownItems.Add(saveMenuItem);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add(exitMenuItem);

            // Меню Правка
            editMenu = new ToolStripMenuItem("Правка");
            var processMenuItem = new ToolStripMenuItem("Обработать", null, ProcessText_Click);
            processMenuItem.ShortcutKeys = Keys.Control | Keys.Enter;
            var clearMenuItem = new ToolStripMenuItem("Очистить", null, ClearText_Click);
            clearMenuItem.ShortcutKeys = Keys.Control | Keys.Delete;
            var copyMenuItem = new ToolStripMenuItem("Копировать результат", null, CopyResult_Click);
            copyMenuItem.ShortcutKeys = Keys.Control | Keys.C;

            editMenu.DropDownItems.Add(processMenuItem);
            editMenu.DropDownItems.Add(clearMenuItem);
            editMenu.DropDownItems.Add(new ToolStripSeparator());
            editMenu.DropDownItems.Add(copyMenuItem);

            // Меню Инструменты
            toolsMenu = new ToolStripMenuItem("Инструменты");
            var statsMenuItem = new ToolStripMenuItem("Статистика", null, ShowStats_Click);
            var formatMenuItem = new ToolStripMenuItem("Форматировать", null, FormatText_Click);
            var findMenuItem = new ToolStripMenuItem("Найти", null, SearchText_Click);
            findMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            var replaceMenuItem = new ToolStripMenuItem("Заменить", null, ReplaceText_Click);
            replaceMenuItem.ShortcutKeys = Keys.Control | Keys.H;

            toolsMenu.DropDownItems.Add(statsMenuItem);
            toolsMenu.DropDownItems.Add(formatMenuItem);
            toolsMenu.DropDownItems.Add(new ToolStripSeparator());
            toolsMenu.DropDownItems.Add(findMenuItem);
            toolsMenu.DropDownItems.Add(replaceMenuItem);

            // Меню Справка
            helpMenu = new ToolStripMenuItem("Справка");
            var aboutMenuItem = new ToolStripMenuItem("О программе", null, About_Click);
            var helpMenuItem = new ToolStripMenuItem("Помощь", null, Help_Click);
            helpMenuItem.ShortcutKeys = Keys.F1;

            helpMenu.DropDownItems.Add(aboutMenuItem);
            helpMenu.DropDownItems.Add(helpMenuItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(editMenu);
            menuStrip.Items.Add(toolsMenu);
            menuStrip.Items.Add(helpMenu);

            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;
        }

        /// <summary>
        /// Создание области ввода
        /// </summary>
        private void CreateInputArea()
        {
            inputLabel = new Label
            {
                Text = "Исходный текст:",
                Location = new System.Drawing.Point(20, 40),
                Size = new System.Drawing.Size(150, 25),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(44, 62, 80)
            };

            inputTextBox = new RichTextBox
            {
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                Location = new System.Drawing.Point(20, 70),
                Size = new System.Drawing.Size(400, 200),
                Font = new System.Drawing.Font("Consolas", 10),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                WordWrap = false
            };

            // Добавляем подсказку
            inputTextBox.Enter += (s, e) =>
            {
                if (inputTextBox.Text == "Введите текст здесь...")
                {
                    inputTextBox.Text = "";
                    inputTextBox.ForeColor = System.Drawing.Color.Black;
                }
            };

            inputTextBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(inputTextBox.Text))
                {
                    inputTextBox.Text = "Введите текст здесь...";
                    inputTextBox.ForeColor = System.Drawing.Color.Gray;
                }
            };

            this.Controls.Add(inputLabel);
            this.Controls.Add(inputTextBox);
        }

        /// <summary>
        /// Создание области вывода
        /// </summary>
        private void CreateOutputArea()
        {
            outputLabel = new Label
            {
                Text = "Обработанный текст:",
                Location = new System.Drawing.Point(450, 40),
                Size = new System.Drawing.Size(170, 25),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(44, 62, 80)
            };

            outputTextBox = new RichTextBox
            {
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                ReadOnly = true,
                Location = new System.Drawing.Point(450, 70),
                Size = new System.Drawing.Size(400, 200),
                Font = new System.Drawing.Font("Consolas", 10),
                BackColor = System.Drawing.Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle,
                WordWrap = false
            };

            this.Controls.Add(outputLabel);
            this.Controls.Add(outputTextBox);
        }

        /// <summary>
        /// Создание области поиска и замены
        /// </summary>
        private void CreateSearchReplaceArea()
        {
            // Группа поиска и замены
            GroupBox searchGroup = new GroupBox
            {
                Text = "Поиск и замена",
                Location = new System.Drawing.Point(20, 330),
                Size = new System.Drawing.Size(340, 120),
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold)
            };

            // Поиск
            searchLabel = new Label
            {
                Text = "Найти:",
                Location = new System.Drawing.Point(15, 25),
                Size = new System.Drawing.Size(50, 20),
                Font = new System.Drawing.Font("Segoe UI", 9)
            };

            searchTextBox = new TextBox
            {
                Location = new System.Drawing.Point(70, 22),
                Size = new System.Drawing.Size(150, 25),
                Font = new System.Drawing.Font("Segoe UI", 9)
            };

            searchButton = new Button
            {
                Text = "Найти",
                Location = new System.Drawing.Point(230, 20),
                Size = new System.Drawing.Size(90, 28),
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold)
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.Click += SearchText_Click;

            // Замена
            replaceLabel = new Label
            {
                Text = "Заменить на:",
                Location = new System.Drawing.Point(15, 65),
                Size = new System.Drawing.Size(80, 20),
                Font = new System.Drawing.Font("Segoe UI", 9)
            };

            replaceTextBox = new TextBox
            {
                Location = new System.Drawing.Point(100, 62),
                Size = new System.Drawing.Size(120, 25),
                Font = new System.Drawing.Font("Segoe UI", 9)
            };

            replaceButton = new Button
            {
                Text = "Заменить все",
                Location = new System.Drawing.Point(230, 60),
                Size = new System.Drawing.Size(90, 28),
                BackColor = System.Drawing.Color.FromArgb(46, 204, 113),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold)
            };
            replaceButton.FlatAppearance.BorderSize = 0;
            replaceButton.Click += ReplaceText_Click;

            // Добавляем элементы в группу
            searchGroup.Controls.Add(searchLabel);
            searchGroup.Controls.Add(searchTextBox);
            searchGroup.Controls.Add(searchButton);
            searchGroup.Controls.Add(replaceLabel);
            searchGroup.Controls.Add(replaceTextBox);
            searchGroup.Controls.Add(replaceButton);

            this.Controls.Add(searchGroup);
        }

        /// <summary>
        /// Создание кнопок управления
        /// </summary>
        private void CreateControlButtons()
        {
            // Панель для кнопок
            Panel buttonPanel = new Panel
            {
                Location = new System.Drawing.Point(20, 280),
                Size = new System.Drawing.Size(830, 40),
                BackColor = System.Drawing.Color.Transparent
            };

            // Кнопка Загрузить
            loadButton = new Button
            {
                Text = "📂 Загрузить",
                Size = new System.Drawing.Size(110, 35),
                Location = new System.Drawing.Point(0, 0),
                BackColor = System.Drawing.Color.FromArgb(46, 204, 113),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            loadButton.FlatAppearance.BorderSize = 0;
            loadButton.Click += LoadButton_Click;

            // Кнопка Обработать
            processButton = new Button
            {
                Text = "⚡ Обработать",
                Size = new System.Drawing.Size(110, 35),
                Location = new System.Drawing.Point(120, 0),
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            processButton.FlatAppearance.BorderSize = 0;
            processButton.Click += ProcessText_Click;

            // Кнопка Сохранить
            saveButton = new Button
            {
                Text = "💾 Сохранить",
                Size = new System.Drawing.Size(110, 35),
                Location = new System.Drawing.Point(240, 0),
                BackColor = System.Drawing.Color.FromArgb(155, 89, 182),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;

            // Кнопка Копировать
            copyButton = new Button
            {
                Text = "📋 Копировать",
                Size = new System.Drawing.Size(110, 35),
                Location = new System.Drawing.Point(360, 0),
                BackColor = System.Drawing.Color.FromArgb(241, 196, 15),
                ForeColor = System.Drawing.Color.Black,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            copyButton.FlatAppearance.BorderSize = 0;
            copyButton.Click += CopyResult_Click;

            // Кнопка Статистика
            statsButton = new Button
            {
                Text = "📊 Статистика",
                Size = new System.Drawing.Size(110, 35),
                Location = new System.Drawing.Point(480, 0),
                BackColor = System.Drawing.Color.FromArgb(230, 126, 34),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            statsButton.FlatAppearance.BorderSize = 0;
            statsButton.Click += ShowStats_Click;

            // Кнопка Форматировать
            formatButton = new Button
            {
                Text = "✨ Форматировать",
                Size = new System.Drawing.Size(120, 35),
                Location = new System.Drawing.Point(600, 0),
                BackColor = System.Drawing.Color.FromArgb(22, 160, 133),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            formatButton.FlatAppearance.BorderSize = 0;
            formatButton.Click += FormatText_Click;

            // Кнопка Очистить
            clearButton = new Button
            {
                Text = "🗑️ Очистить",
                Size = new System.Drawing.Size(90, 35),
                Location = new System.Drawing.Point(730, 0),
                BackColor = System.Drawing.Color.FromArgb(231, 76, 60),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            clearButton.FlatAppearance.BorderSize = 0;
            clearButton.Click += ClearText_Click;

            // Добавляем кнопки на панель
            buttonPanel.Controls.Add(loadButton);
            buttonPanel.Controls.Add(processButton);
            buttonPanel.Controls.Add(saveButton);
            buttonPanel.Controls.Add(copyButton);
            buttonPanel.Controls.Add(statsButton);
            buttonPanel.Controls.Add(formatButton);
            buttonPanel.Controls.Add(clearButton);

            this.Controls.Add(buttonPanel);
        }

        /// <summary>
        /// Создание области статистики
        /// </summary>
        private void CreateStatsArea()
        {
            // Группа статистики
            GroupBox statsGroup = new GroupBox
            {
                Text = "Статистика",
                Location = new System.Drawing.Point(400, 460),
                Size = new System.Drawing.Size(450, 150),
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold)
            };

            statsLabel = new Label
            {
                Text = "Текст не обработан\n\n" +
                      "Символов: 0\n" +
                      "Слов: 0\n" +
                      "Строк: 0\n" +
                      "Уникальных слов: 0",
                Location = new System.Drawing.Point(15, 25),
                Size = new System.Drawing.Size(420, 110),
                Font = new System.Drawing.Font("Consolas", 9),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.FromArgb(248, 249, 250),
                Padding = new Padding(10)
            };

            statsGroup.Controls.Add(statsLabel);
            this.Controls.Add(statsGroup);
        }

        /// <summary>
        /// Настройка формы
        /// </summary>
        private void SetupForm()
        {
            // Добавляем обработчик закрытия формы
            this.FormClosing += MainForm_FormClosing;

            // Устанавливаем начальный текст подсказки
            if (string.IsNullOrWhiteSpace(inputTextBox.Text))
            {
                inputTextBox.Text = "Введите текст здесь...";
                inputTextBox.ForeColor = System.Drawing.Color.Gray;
            }
        }

        // ============= ОБРАБОТЧИКИ СОБЫТИЙ =============

        /// <summary>
        /// Обработчик закрытия формы
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(inputTextBox.Text) &&
                inputTextBox.Text != "Введите текст здесь...")
            {
                var result = MessageBox.Show(
                    "Сохранить изменения перед выходом?",
                    "Подтверждение",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveButton_Click(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Открыть файл из меню
        /// </summary>
        private void OpenFile_Click(object sender, EventArgs e)
        {
            LoadButton_Click(sender, e);
        }

        /// <summary>
        /// Сохранить файл из меню
        /// </summary>
        private void SaveFile_Click(object sender, EventArgs e)
        {
            SaveButton_Click(sender, e);
        }

        /// <summary>
        /// Выход из программы
        /// </summary>
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// О программе
        /// </summary>
        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "TextProcessorVS - Система обработки текстов\n" +
                "Версия 1.0.0\n\n" +
                "Автор: Иванов Иван Иванович\n" +
                "Группа: ПИ-2023\n" +
                "Преподаватель: Петров П.П.\n\n" +
                "Кафедра программной инженерии\n" +
                "Национальный исследовательский университет\n\n" +
                "© 2024 Все права защищены",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Помощь
        /// </summary>
        private void Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "ИНСТРУКЦИЯ ПО ИСПОЛЬЗОВАНИЮ:\n\n" +
                "1. ВВОД ТЕКСТА:\n" +
                "   - Введите текст в левое поле\n" +
                "   - Или нажмите 'Загрузить' для открытия файла\n\n" +
                "2. ОБРАБОТКА ТЕКСТА:\n" +
                "   - 'Обработать' - базовая обработка текста\n" +
                "   - 'Форматировать' - улучшение форматирования\n\n" +
                "3. ПОИСК И ЗАМЕНА:\n" +
                "   - Введите текст для поиска\n" +
                "   - Нажмите 'Найти' (подсветка желтым)\n" +
                "   - Для замены введите новый текст\n" +
                "   - Нажмите 'Заменить все'\n\n" +
                "4. АНАЛИЗ:\n" +
                "   - 'Статистика' - подробная информация о тексте\n" +
                "   - Статистика обновляется автоматически\n\n" +
                "5. СОХРАНЕНИЕ:\n" +
                "   - 'Сохранить' - сохранить результат\n" +
                "   - 'Копировать' - скопировать в буфер обмена\n\n" +
                "Горячие клавиши:\n" +
                "Ctrl+O - Открыть файл\n" +
                "Ctrl+S - Сохранить\n" +
                "Ctrl+F - Найти\n" +
                "Ctrl+H - Заменить\n" +
                "F1 - Помощь",
                "Помощь",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Загрузка файла
        /// </summary>
        private void LoadButton_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли несохраненные изменения
            if (!string.IsNullOrWhiteSpace(inputTextBox.Text) &&
                inputTextBox.Text != "Введите текст здесь...")
            {
                var dialogResult = MessageBox.Show(
                    "Текущий текст будет заменен. Продолжить?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                    inputTextBox.Text = fileContent;
                    inputTextBox.ForeColor = System.Drawing.Color.Black;

                    // Сбрасываем подсветку
                    inputTextBox.SelectAll();
                    inputTextBox.SelectionBackColor = System.Drawing.Color.White;
                    inputTextBox.Select(0, 0);

                    // Обновляем статистику
                    UpdateStatistics(fileContent);

                    MessageBox.Show(
                        $"Файл успешно загружен!\n\n" +
                        $"Имя: {Path.GetFileName(openFileDialog.FileName)}\n" +
                        $"Размер: {fileContent.Length} символов",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Ошибка загрузки файла:\n{ex.Message}",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Обработка текста
        /// </summary>
        private void ProcessText_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputTextBox.Text) ||
                inputTextBox.Text == "Введите текст здесь...")
            {
                MessageBox.Show(
                    "Введите текст для обработки!",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string textToProcess = inputTextBox.Text;
                string processedText = ProcessText(textToProcess);
                outputTextBox.Text = processedText;

                UpdateStatistics(textToProcess);

                MessageBox.Show(
                    "Текст успешно обработан!",
                    "Успех",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка обработки текста:\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Сохранение результата
        /// </summary>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(outputTextBox.Text))
            {
                MessageBox.Show(
                    "Нет данных для сохранения! Сначала обработайте текст.",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, outputTextBox.Text, Encoding.UTF8);

                    MessageBox.Show(
                        $"Файл успешно сохранен!\n\n" +
                        $"Путь: {saveFileDialog.FileName}",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Ошибка сохранения файла:\n{ex.Message}",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Поиск текста с подсветкой
        /// </summary>
        private void SearchText_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                MessageBox.Show(
                    "Введите текст для поиска!",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(inputTextBox.Text) ||
                inputTextBox.Text == "Введите текст здесь...")
            {
                MessageBox.Show(
                    "Нет текста для поиска!",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string searchText = searchTextBox.Text;
            string sourceText = inputTextBox.Text;

            // Сбрасываем предыдущую подсветку
            inputTextBox.SelectAll();
            inputTextBox.SelectionBackColor = System.Drawing.Color.White;

            int count = CountOccurrences(sourceText, searchText);
            int index = 0;

            // Ищем и подсвечиваем все вхождения
            while ((index = sourceText.IndexOf(searchText, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                inputTextBox.Select(index, searchText.Length);
                inputTextBox.SelectionBackColor = System.Drawing.Color.Yellow;
                index += searchText.Length;
            }

            // Снимаем выделение курсора
            inputTextBox.Select(0, 0);

            if (count > 0)
            {
                statsLabel.Text = $"Найдено вхождений '{searchText}': {count}";

                // Показываем позиции в outputTextBox
                StringBuilder positions = new StringBuilder();
                int posIndex = 0;
                int found = 0;

                while ((posIndex = sourceText.IndexOf(searchText, posIndex, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    found++;
                    positions.AppendLine($"  {found}. Позиция {posIndex}");
                    posIndex += searchText.Length;
                }

                outputTextBox.Text = $"=== РЕЗУЛЬТАТЫ ПОИСКА ===\n\n" +
                                   $"Искомый текст: '{searchText}'\n" +
                                   $"Всего найдено: {count} вхождений\n\n" +
                                   $"Позиции в тексте:\n" +
                                   positions.ToString();

                MessageBox.Show(
                    $"Найдено вхождений: {count}\n\n" +
                    $"Текст подсвечен желтым цветом в исходном тексте.",
                    "Результат поиска",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    $"Текст '{searchText}' не найден",
                    "Результат поиска",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Замена текста
        /// </summary>
        private void ReplaceText_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                MessageBox.Show(
                    "Введите текст для замены!",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(inputTextBox.Text) ||
                inputTextBox.Text == "Введите текст здесь...")
            {
                MessageBox.Show(
                    "Нет текста для обработки!",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string oldText = searchTextBox.Text;
            string newText = replaceTextBox.Text ?? "";
            string sourceText = inputTextBox.Text;

            // Подсчитываем количество вхождений
            int occurrences = CountOccurrences(sourceText, oldText);

            if (occurrences == 0)
            {
                MessageBox.Show(
                    $"Текст '{oldText}' не найден для замены.",
                    "Информация",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // Спрашиваем подтверждение
            var result = MessageBox.Show(
                $"Заменить '{oldText}' на '{newText}'?\n" +
                $"Будет заменено: {occurrences} вхождений",
                "Подтверждение замены",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Выполняем замену (без учета регистра)
                string replacedText = ReplaceTextIgnoreCase(sourceText, oldText, newText);
                inputTextBox.Text = replacedText;
                inputTextBox.ForeColor = System.Drawing.Color.Black;

                // Сбрасываем подсветку
                inputTextBox.SelectAll();
                inputTextBox.SelectionBackColor = System.Drawing.Color.White;
                inputTextBox.Select(0, 0);

                UpdateStatistics(replacedText);

                MessageBox.Show(
                    $"Замена выполнена успешно!\n" +
                    $"Изменено: {occurrences} вхождений",
                    "Успех",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Очистка текста
        /// </summary>
        private void ClearText_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Очистить все текстовые поля?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                inputTextBox.Clear();
                inputTextBox.Text = "Введите текст здесь...";
                inputTextBox.ForeColor = System.Drawing.Color.Gray;
                outputTextBox.Clear();
                searchTextBox.Clear();
                replaceTextBox.Clear();
                statsLabel.Text = "Текст не обработан\n\n" +
                                "Символов: 0\n" +
                                "Слов: 0\n" +
                                "Строк: 0\n" +
                                "Уникальных слов: 0";
            }
        }

        /// <summary>
        /// Копирование результата в буфер обмена
        /// </summary>
        private void CopyResult_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(outputTextBox.Text))
            {
                Clipboard.SetText(outputTextBox.Text);
                MessageBox.Show(
                    "Результат скопирован в буфер обмена!",
                    "Успех",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Нет данных для копирования!",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Показать статистику
        /// </summary>
        private void ShowStats_Click(object sender, EventArgs e)
        {
            string text = inputTextBox.Text;

            if (string.IsNullOrWhiteSpace(text) || text == "Введите текст здесь...")
            {
                MessageBox.Show(
                    "Нет текста для анализа!",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            UpdateStatistics(text);

            MessageBox.Show(
                $"СТАТИСТИКА ТЕКСТА:\n\n" +
                $"Символов: {text.Length}\n" +
                $"Символов (без пробелов): {text.Replace(" ", "").Length}\n" +
                $"Слов: {CountWords(text)}\n" +
                $"Строк: {CountLines(text)}\n" +
                $"Предложений: {CountSentences(text)}\n" +
                $"Уникальных слов: {CountUniqueWords(text)}\n" +
                $"Средняя длина слова: {CalculateAverageWordLength(text):F1} символов",
                "Статистика",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Форматирование текста
        /// </summary>
        private void FormatText_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputTextBox.Text) ||
                inputTextBox.Text == "Введите текст здесь...")
            {
                MessageBox.Show(
                    "Нет текста для форматирования!",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string formattedText = FormatText(inputTextBox.Text);
            outputTextBox.Text = formattedText;

            MessageBox.Show(
                "Текст отформатирован!",
                "Успех",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ============= МЕТОДЫ ОБРАБОТКИ ТЕКСТА =============

        /// <summary>
        /// Основная обработка текста
        /// </summary>
        private string ProcessText(string text)
        {
            StringBuilder result = new StringBuilder();

            // 1. Удаляем лишние пробелы
            string step1 = Regex.Replace(text, @"\s+", " ");

            // 2. Удаляем пробелы в начале и конце строк
            string step2 = Regex.Replace(step1, @"^\s+|\s+$", "", RegexOptions.Multiline);

            // 3. Делаем заглавные буквы в начале предложений
            string step3 = CapitalizeSentences(step2);

            // 4. Форматируем абзацы
            string step4 = FormatParagraphs(step3);

            result.AppendLine("=== ОБРАБОТАННЫЙ ТЕКСТ ===");
            result.AppendLine();
            result.AppendLine(step4);
            result.AppendLine();
            result.AppendLine("=== СТАТИСТИКА ===");
            result.AppendLine($"Символов: {text.Length}");
            result.AppendLine($"Символов (без пробелов): {text.Replace(" ", "").Length}");
            result.AppendLine($"Слов: {CountWords(text)}");
            result.AppendLine($"Строк: {CountLines(text)}");
            result.AppendLine($"Уникальных слов: {CountUniqueWords(text)}");
            result.AppendLine($"Средняя длина слова: {CalculateAverageWordLength(text):F1} символов");

            return result.ToString();
        }

        /// <summary>
        /// Подсчет вхождений подстроки
        /// </summary>
        private int CountOccurrences(string text, string search)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(search))
                return 0;

            int count = 0;
            int index = 0;

            while ((index = text.IndexOf(search, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                index += search.Length;
            }

            return count;
        }

        /// <summary>
        /// Замена текста без учета регистра
        /// </summary>
        private string ReplaceTextIgnoreCase(string text, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(oldValue))
                return text;

            int index = 0;
            StringBuilder result = new StringBuilder(text);

            while ((index = text.IndexOf(oldValue, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                result.Remove(index, oldValue.Length);
                result.Insert(index, newValue);

                text = result.ToString();
                index += newValue.Length;
            }

            return result.ToString();
        }

        /// <summary>
        /// Подсчет слов
        /// </summary>
        private int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            char[] delimiters = new char[] { ' ', '\r', '\n', '\t', '.', ',', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}', '"', '\'' };
            string[] words = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        /// <summary>
        /// Подсчет строк
        /// </summary>
        private int CountLines(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            int count = 1;
            foreach (char c in text)
            {
                if (c == '\n')
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Подсчет предложений
        /// </summary>
        private int CountSentences(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            string[] sentences = Regex.Split(text, @"(?<=[.!?])\s+");
            return sentences.Length;
        }

        /// <summary>
        /// Подсчет уникальных слов
        /// </summary>
        private int CountUniqueWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            char[] delimiters = new char[] { ' ', '\r', '\n', '\t', '.', ',', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}', '"', '\'' };
            string[] words = text.ToLower().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            var uniqueWords = new HashSet<string>(words);
            return uniqueWords.Count;
        }

        /// <summary>
        /// Расчет средней длины слова
        /// </summary>
        private double CalculateAverageWordLength(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            char[] delimiters = new char[] { ' ', '\r', '\n', '\t', '.', ',', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}', '"', '\'' };
            string[] words = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0)
                return 0;

            int totalLength = 0;
            foreach (string word in words)
            {
                totalLength += word.Length;
            }

            return (double)totalLength / words.Length;
        }

        /// <summary>
        /// Заглавные буквы в предложениях
        /// </summary>
        private string CapitalizeSentences(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            StringBuilder result = new StringBuilder();
            bool newSentence = true;

            foreach (char c in text)
            {
                if (newSentence && char.IsLetter(c))
                {
                    result.Append(char.ToUpper(c));
                    newSentence = false;
                }
                else
                {
                    result.Append(c);
                }

                if (c == '.' || c == '!' || c == '?')
                {
                    newSentence = true;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Форматирование абзацев
        /// </summary>
        private string FormatParagraphs(string text)
        {
            StringBuilder result = new StringBuilder();
            string[] paragraphs = text.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None);

            foreach (string paragraph in paragraphs)
            {
                string trimmed = paragraph.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    // Удаляем переносы строк внутри абзаца
                    string singleLine = trimmed.Replace("\r\n", " ").Replace("\n", " ");

                    // Удаляем лишние пробелы
                    singleLine = Regex.Replace(singleLine, @"\s+", " ");

                    result.AppendLine(singleLine.Trim());
                    result.AppendLine(); // Пустая строка между абзацами
                }
            }

            return result.ToString().TrimEnd();
        }

        /// <summary>
        /// Форматирование текста
        /// </summary>
        private string FormatText(string text)
        {
            StringBuilder result = new StringBuilder();

            // Разбиваем на строки
            string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            bool firstLine = true;
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    // Делаем первую букву заглавной
                    if (trimmedLine.Length > 0)
                    {
                        trimmedLine = char.ToUpper(trimmedLine[0]) + trimmedLine.Substring(1);
                    }

                    // Добавляем точку в конце, если нет знака препинания
                    if (!trimmedLine.EndsWith(".") &&
                        !trimmedLine.EndsWith("!") &&
                        !trimmedLine.EndsWith("?") &&
                        !trimmedLine.EndsWith(":") &&
                        !trimmedLine.EndsWith(";") &&
                        !trimmedLine.EndsWith(","))
                    {
                        trimmedLine += ".";
                    }

                    if (!firstLine)
                    {
                        result.AppendLine();
                    }
                    result.Append(trimmedLine);
                    firstLine = false;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Обновление статистики
        /// </summary>
        private void UpdateStatistics(string text)
        {
            if (string.IsNullOrEmpty(text) || text == "Введите текст здесь...")
            {
                statsLabel.Text = "Текст не обработан\n\n" +
                                "Символов: 0\n" +
                                "Слов: 0\n" +
                                "Строк: 0\n" +
                                "Уникальных слов: 0";
                return;
            }

            int words = CountWords(text);
            int lines = CountLines(text);
            int sentences = CountSentences(text);
            int uniqueWords = CountUniqueWords(text);
            double avgWordLength = CalculateAverageWordLength(text);

            statsLabel.Text = $"Текст обработан\n\n" +
                             $"Символов: {text.Length}\n" +
                             $"Слов: {words}\n" +
                             $"Строк: {lines}\n" +
                             $"Предложений: {sentences}\n" +
                             $"Уникальных слов: {uniqueWords}\n" +
                             $"Средняя длина слова: {avgWordLength:F1}";
        }
    }
}