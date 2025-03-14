namespace lapa2._1;

class Program
{
    static Mutex mutex = new Mutex();
    static int sharedResource = 0;

    static void Worker()
    {
        for (int i = 0; i < 5; i++)
        {
            mutex.Lock();
            
            int current = sharedResource;
            Thread.Sleep(00);
            sharedResource = current + 1;

            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} увеличил sharedResource до {sharedResource}");

            mutex.Unlock();
        }
    }

    static void Main()
    {
        Thread t1 = new Thread(Worker);
        Thread t2 = new Thread(Worker);
        Thread t3 = new Thread(Worker);

        t1.Start();
        t2.Start();
        t3.Start();

        t1.Join();
        t2.Join();
        t3.Join();

        Console.WriteLine($"Финальное значение sharedResource: {sharedResource}");
    }
}