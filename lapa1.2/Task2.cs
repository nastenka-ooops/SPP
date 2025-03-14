using System.Diagnostics;

namespace lapa1._2;

class Task2
{
    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Использование: copyapp <исходный каталог> <целевой каталог>");
            return;
        }

        string sourceDir = args[0];
        string targetDir = args[1];

        if (!Directory.Exists(sourceDir))
        {
            Console.WriteLine($"Ошибка: Исходный каталог '{sourceDir}' не существует.");
            return;
        }

        Directory.CreateDirectory(targetDir);
        Console.WriteLine($"[INFO] Копирование файлов из '{sourceDir}' в '{targetDir}'...");

        // Запуск таймера
        Stopwatch stopwatch = Stopwatch.StartNew();

        CopyFilesParallel(sourceDir, targetDir);

        stopwatch.Stop();

        Console.WriteLine($"[INFO] Время выполнения: {stopwatch.Elapsed.TotalSeconds:F2} секунд");
    }
    
    static void CopyFilesParallel(string sourceDir, string targetDir)
    {
        string[] files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
        int copiedCount = 0;

        Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, file =>
        {
            string relativePath = file.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar);
            string targetFilePath = Path.Combine(targetDir, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

            try
            {
                File.Copy(file, targetFilePath, true);
                int count = Interlocked.Increment(ref copiedCount);
                Console.WriteLine($"[INFO] Скопирован: {file} -> {targetFilePath} ({count}/{files.Length})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ошибка копирования {file}: {ex.Message}");
            }
        });

        Console.WriteLine($"[INFO] Копирование завершено. Всего файлов скопировано: {copiedCount}");
    }
}