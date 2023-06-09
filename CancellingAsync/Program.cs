﻿class Program
{
    static void Main()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        MyClass mc = new MyClass();
        Task t = mc.RunAsync(token);
        //Thread.Sleep(3000); // Wait 3 seconds.
        //cts.Cancel(); //cancel the operation.
        t.Wait();
        Console.WriteLine("Was Cancelled: {0}", token.IsCancellationRequested);
    }
}
class MyClass
{
    public async Task RunAsync(CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
            return;
        await Task.Run(() => CycleMethod(ct), ct);
    }
    void CycleMethod(CancellationToken ct)
    {
        Console.WriteLine("Starting CycleMethod");
        const int max = 5;
        for (int i = 0; i < max; i++)
        {
            if (ct.IsCancellationRequested) // Monitor the CancellationToken.
                return;
            Thread.Sleep(1000);
            Console.WriteLine(" {0} of {1} iterations completed", i + 1, max);
        }
    }
}