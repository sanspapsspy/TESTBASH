using System;
using System.Windows.Forms;
using TESTBASH;

namespace TextProcessorVS
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
            Console.WriteLine("ПРЕПОДАВАТЕЛЬ: Петров П.П.");
            Console.WriteLine("КАФЕДРА: Программной инженерии");
            Console.WriteLine("ВУЗ: Национальный исследовательский университет");
            Console.WriteLine("========================================");
            Console.WriteLine("ВОЗМОЖНОСТИ ПРИЛОЖЕНИЯ:");
            Console.WriteLine("1. Обработка текстовых файлов");
            Console.WriteLine("2. Подсчет статистики текста");
            Console.WriteLine("3. Поиск и замена текста");
            Console.WriteLine("4. Форматирование текста");
            Console.WriteLine("========================================\n");

            // Если есть аргументы командной строки
            if (args.Length > 0)
            {
                if (args[0] == "-h" || args[0] == "--help")
                {
                    ShowHelp();
                    return;
                }
                else if (args[0] == "-m" || args[0] == "--menu")
                {
                    // Запускаем графический интерфейс
                    RunGUI();
                }
            }
            else
            {
                // Запуск GUI по умолчанию
                RunGUI();
            }
        }

        /// <summary>
        /// Показать справку по использованию
        /// </summary>
        static void ShowHelp()
        {
            Console.WriteLine("\nИСПОЛЬЗОВАНИЕ: TextProcessorVS [ОПЦИИ]");
            Console.WriteLine("\nОПЦИИ:");
            Console.WriteLine("  -h, --help     Показать эту справку");
            Console.WriteLine("  -m, --menu     Запустить графический интерфейс");
            Console.WriteLine("  -f ФАЙЛ        Обработать указанный файл");
            Console.WriteLine("  -o ФАЙЛ        Сохранить результат в файл");
            Console.WriteLine("\nПРИМЕРЫ:");
            Console.WriteLine("  TextProcessorVS -m                (графический режим)");
            Console.WriteLine("  TextProcessorVS -f input.txt      (обработать файл)");
            Console.WriteLine("  TextProcessorVS -h                (справка)");
        }

        /// <summary>
        /// Запуск графического интерфейса
        /// </summary>
        static void RunGUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}