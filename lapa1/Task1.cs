namespace lapa1;

class Task1
{
    public static void Main(string[] args)
    {
        string? s = Console.ReadLine();
        int threadCount = s == null ? 0 : int.Parse(s);
        if (threadCount < 0)
        {
            Console.WriteLine("Thread count cannot be negative");
            return;
        }
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