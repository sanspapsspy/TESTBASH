using System;
using System.IO;
using System.Windows.Forms;

namespace TESTBASH
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Вывод информации об авторе в консоль при запуске
            Console.WriteLine("========================================");
            Console.WriteLine("АВТОР: Устинов Александр Юрьевич");
            Console.WriteLine("ГРУППА: ИТП-125");
            Console.WriteLine("ПРЕПОДАВАТЕЛЬ: ");
            Console.WriteLine("КАФЕДРА: ");
            Console.WriteLine("ВУЗ: ");
            Console.WriteLine("========================================");
            Console.WriteLine("ВОЗМОЖНОСТИ ПРИЛОЖЕНИЯ:");
            Console.WriteLine("1. Обработка текстовых файлов");
            Console.WriteLine("2. Подсчет статистики текста");
            Console.WriteLine("3. Поиск и замена текста");
            Console.WriteLine("4. Форматирование текста");
            Console.WriteLine("========================================\n");

            // Обработка аргументов командной строки
            if (args.Length > 0)
            {
                ProcessCommandLineArgs(args);
                return;
            }
            
            // Запуск в интерактивном режиме
            RunInteractiveMode();
        }

        /// <summary>
        /// Обработка аргументов командной строки
        /// </summary>
        static void ProcessCommandLineArgs(string[] args)
        {
            string configFile = null;
            bool detailedLogging = false;
            string inputFile = null;
            bool showHelp = false;
            bool showInfo = false;
            bool menuMode = false;
            string outputFile = null;
            bool silentMode = false;
            bool keepTempFiles = false;

            // Парсинг аргументов
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-c":
                        if (i + 1 < args.Length)
                            configFile = args[++i];
                        break;
                    case "-d":
                        detailedLogging = true;
                        break;
                    case "-f":
                        if (i + 1 < args.Length)
                            inputFile = args[++i];
                        break;
                    case "-h":
                        showHelp = true;
                        break;
                    case "-i":
                        showInfo = true;
                        break;
                    case "-m":
                        menuMode = true;
                        break;
                    case "-o":
                        if (i + 1 < args.Length)
                            outputFile = args[++i];
                        break;
                    case "-s":
                        silentMode = true;
                        break;
                    case "-t":
                        keepTempFiles = true;
                        break;
                }
            }

            // Выполнение соответствующих действий
            if (showHelp)
            {
                ShowHelp();
            }
            else if (showInfo)
            {
                ShowSystemInfo();
            }
            else if (menuMode || args.Length == 0)
            {
                RunGUI(inputFile, outputFile);
            }
            else if (inputFile != null)
            {
                ProcessFileInConsole(inputFile, outputFile);
            }
            else
            {
                Console.WriteLine("Используйте -h для справки");
                RunInteractiveMode();
            }
        }

        /// <summary>
        /// Показать справку
        /// </summary>
        static void ShowHelp()
        {
            Console.WriteLine("ИСПОЛЬЗОВАНИЕ: TESTBASH [ОПЦИИ]");
            Console.WriteLine();
            Console.WriteLine("ОПЦИИ:");
            Console.WriteLine("  -c ФАЙЛ     Запуск с указанным конфигурационным файлом");
            Console.WriteLine("  -d          Подробное логирование диалога пользователя");
            Console.WriteLine("  -f ФАЙЛ     Обработать указанный файл");
            Console.WriteLine("  -h          Показать эту справку");
            Console.WriteLine("  -i          Информация об авторе и системе");
            Console.WriteLine("  -m          Запуск в режиме меню (графический интерфейс)");
            Console.WriteLine("  -o ФАЙЛ     Файл для сохранения результата");
            Console.WriteLine("  -s          Тихий режим (без логов)");
            Console.WriteLine("  -t          Не удалять временные файлы");
            Console.WriteLine();
            Console.WriteLine("ПРИМЕРЫ:");
            Console.WriteLine("  TESTBASH -f input.txt -o output.txt");
            Console.WriteLine("  TESTBASH -m");
            Console.WriteLine("  TESTBASH -c myconfig.conf");
        }

        /// <summary>
        /// Показать информацию о системе
        /// </summary>
        static void ShowSystemInfo()
        {
            Console.WriteLine("ИНФОРМАЦИЯ О СИСТЕМЕ");
            Console.WriteLine("====================");
            Console.WriteLine($"Версия программы: 1.0.0");
            Console.WriteLine($"Операционная система: {Environment.OSVersion}");
            Console.WriteLine($"Пользователь: {Environment.UserName}");
            Console.WriteLine($"Дата и время: {DateTime.Now}");
            Console.WriteLine($"Текущая директория: {Environment.CurrentDirectory}");
            Console.WriteLine();
            Console.WriteLine("Сертификат: [Информация о сертификате]");
        }

        /// <summary>
        /// Обработка файла в консольном режиме
        /// </summary>
        static void ProcessFileInConsole(string inputFile, string outputFile)
        {
            try
            {
                if (!File.Exists(inputFile))
                {
                    Console.WriteLine($"Ошибка: Файл '{inputFile}' не найден!");
                    return;
                }

                Console.WriteLine($"Обработка файла: {inputFile}");
                string content = File.ReadAllText(inputFile);
                
                // Простая обработка текста
                int charCount = content.Length;
                int wordCount = content.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
                int lineCount = content.Split('\n').Length;

                Console.WriteLine("\nСТАТИСТИКА:");
                Console.WriteLine($"Символов: {charCount}");
                Console.WriteLine($"Слов: {wordCount}");
                Console.WriteLine($"Строк: {lineCount}");

                if (!string.IsNullOrEmpty(outputFile))
                {
                    File.WriteAllText(outputFile, $"Статистика файла: {inputFile}\n" +
                                                 $"Символов: {charCount}\n" +
                                                 $"Слов: {wordCount}\n" +
                                                 $"Строк: {lineCount}\n" +
                                                 $"Дата обработки: {DateTime.Now}");
                    Console.WriteLine($"\nРезультат сохранен в: {outputFile}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Интерактивный режим
        /// </summary>
        static void RunInteractiveMode()
        {
            Console.WriteLine("ИНТЕРАКТИВНЫЙ РЕЖИМ");
            Console.WriteLine("===================\n");
            
            while (true)
            {
                Console.Write("Введите команду (help - справка, exit - выход): ");
                string command = Console.ReadLine()?.Trim().ToLower();
                
                switch (command)
                {
                    case "help":
                        ShowHelp();
                        break;
                    case "exit":
                        Console.WriteLine("Выход из программы...");
                        return;
                    case "gui":
                        RunGUI(null, null);
                        break;
                    case "info":
                        ShowSystemInfo();
                        break;
                    default:
                        if (!string.IsNullOrEmpty(command))
                        {
                            Console.WriteLine($"Неизвестная команда: {command}");
                        }
                        break;
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Запуск графического интерфейса
        /// </summary>
        static void RunGUI(string inputFile, string outputFile)
        {
            Console.WriteLine("Запуск графического интерфейса...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var mainForm = new MainForm();
            
            // Если переданы файлы, открыть их
            if (!string.IsNullOrEmpty(inputFile) && File.Exists(inputFile))
            {
                // Здесь можно передать файл в форму
            }
            
            Application.Run(mainForm);
        }
    }
}
