namespace lapa1;

class Program
{
    public static void Main(string[] args)
    {
        string? s = Console.ReadLine();
        int threadCount = s == null ? 0 : int.Parse(s);
        TaskQueue taskQueue = new TaskQueue(threadCount);

        for (int i = 0; i < 10; i++)
        {
            int taskNumber = i;
            taskQueue.AddTask(() =>
            {
                Console.WriteLine($"Running task {taskNumber} on thread {Thread.CurrentThread.ManagedThreadId}");
            });
        }
        
        Thread.Sleep(2000);
        taskQueue.Stop();
    }
}