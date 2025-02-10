using System.Collections.Concurrent;

namespace lapa1;

public delegate void TaskDelegate();

public class TaskQueue
{
    private Thread[] threads;
    private BlockingCollection<TaskDelegate> taskQueue;
    private bool running;

    public TaskQueue(int threadCount)
    {
        threads = new Thread[threadCount];
        taskQueue = new BlockingCollection<TaskDelegate>();
        running = true;

        for (int i = 0; i < threadCount; i++)
        {
            threads[i] = new Thread(RunTasks)
            {
                //object initializer
                IsBackground = true
            };
            threads[i].Start();
        }
    }

    private void RunTasks()
    {
        foreach (var task in taskQueue.GetConsumingEnumerable())
        {
            try
            {
                task();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполнения задачи: {ex.Message}");
            }
        }
    }

    public void AddTask(TaskDelegate task)
    {
        if (!running)
        {
            throw new InvalidOperationException("Thread pool is stopped.");
        }

        taskQueue.Add(task);
    }

    public void Stop()
    {
        running = false;
        taskQueue.CompleteAdding();
        foreach (var thread in threads)
        {
            thread.Join();
        }
    }
}