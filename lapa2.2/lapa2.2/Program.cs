
using System.Runtime.InteropServices;

namespace lapa2._2;

class Program
{
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern IntPtr CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool WriteFile(
        IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite,
        out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
    
    private const uint GENERIC_READ = 0x80000000;
    private const uint GENERIC_WRITE = 0x40000000;
    private const uint FILE_SHARE_READ = 0x00000001;
    private const uint FILE_SHARE_WRITE = 0x00000002;
    private const uint OPEN_ALWAYS = 4;
    private const uint FILE_ATTRIBUTE_NORMAL = 0x80;
    
    static void Main()
    {
        string filePath = "C:\\BSUIR\\MoPsP\\lapa2.2\\lapa2.2\\file.txt";
        File.WriteAllText(filePath, "Hello, world!"); // Создадим файл заранее

        IntPtr fileHandle = CreateFile(filePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE,
            IntPtr.Zero, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
        
        if (fileHandle == IntPtr.Zero || fileHandle == new IntPtr(-1))
        {
            Console.WriteLine("Ошибка открытия файла.");
            return;
        }

        Console.WriteLine($"Получен дескриптор: {fileHandle}");
        
        using (OsHandle osHandle = new OsHandle(fileHandle))
        {
            Console.WriteLine($"Освобождение ресурса {osHandle.Handle} через using...");
        }

        byte[] data = System.Text.Encoding.UTF8.GetBytes("Test data");
        if (!WriteFile(fileHandle, data, (uint)data.Length, out uint bytesWritten, IntPtr.Zero))
        {
            Console.WriteLine($"Ошибка при записи в закрытый дескриптор! Код ошибки: {Marshal.GetLastWin32Error()}");
        }
        else
        {
            Console.WriteLine("Неожиданно запись прошла успешно! (Это не должно было случиться)");
        }
    }
}