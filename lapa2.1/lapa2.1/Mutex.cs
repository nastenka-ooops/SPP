namespace lapa2._1;

public class Mutex
{
    private int state = 0; // 0 - свободен, 1 - занят

    public void Lock()
    {
        while (Interlocked.CompareExchange(ref state, 1, 0) != 0)
        {
            Thread.Yield(); // Уступаем процессор другим потокам
        }
    }

    public void Unlock()
    {
        Interlocked.Exchange(ref state, 0);
    }

}