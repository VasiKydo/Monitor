using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ProcessMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: monitor.exe process_name max_lifetime monitoring_frequency");
                return;
            }

            string processName = args[0];
            int maxLifetime = int.Parse(args[1]);
            int monitoringFrequency = int.Parse(args[2]);

            string logFilePath = Path.Combine(Environment.CurrentDirectory, "log.txt");

            Console.WriteLine($"Monitoring process '{processName}' with max lifetime of {maxLifetime} minutes and monitoring frequency of {monitoringFrequency} minutes.");
            Console.WriteLine($"Logging to {logFilePath}. Press 'q' to quit.");

            while (true)
            {
                Process[] processes = Process.GetProcessesByName(processName);

                foreach (Process process in processes)
                {
                    TimeSpan lifetime = DateTime.Now - process.StartTime;

                    if (lifetime.TotalMinutes > maxLifetime)
                    {
                        Console.WriteLine($"Killing process '{process.ProcessName}' with PID {process.Id} (lifetime: {lifetime.TotalMinutes} minutes).");
                        process.Kill();

                        using (StreamWriter sw = File.AppendText(logFilePath))
                        {
                            sw.WriteLine($"Killed process '{process.ProcessName}' with PID {process.Id} (lifetime: {lifetime.TotalMinutes} minutes) at {DateTime.Now}.");
                        }
                    }
                }

                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    Console.WriteLine("Exiting...");
                    return;
                }

                Thread.Sleep(monitoringFrequency * 60 * 1000);
            }
        }
    }
}
