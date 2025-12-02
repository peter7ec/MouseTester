using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        try { Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime; } catch { }

        var app = new MouseTesterApp();

        app.Run();
    }
}