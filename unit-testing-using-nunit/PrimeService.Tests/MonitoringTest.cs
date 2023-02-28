using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using Prime.Services;

namespace Prime.UnitTests.Services
{
    [TestFixture]
    public class MonitoringTest
    {
          [Test] // The first test checks that the log file is created when the program is running
        public void TestLogFileCreation()
        {
            string logFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "log.txt");

            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

            Program.Main(new[] { "Chrome", "1", "1" });

            Assert.That(File.Exists(logFilePath), Is.True);
        }

        [Test]
        // The second test checks that the program kills a process that has been running for 0 minutes (immediately) 
        // and logs the killing action to the log file.
        public void TestProcessKilling()
        {
            Program.Main(new[] { "notepad", "0", "1" });

            // Wait for Chrome to start
            System.Threading.Thread.Sleep(1000);

            int processCountBefore = System.Diagnostics.Process.GetProcessesByName("notepad").Length;

            // Wait for Chrome to run for 2 minutes
            System.Threading.Thread.Sleep(2 * 60 * 1000);

            int processCountAfter = System.Diagnostics.Process.GetProcessesByName("notepad").Length;

            Assert.That(processCountBefore > 0, Is.True);
            Assert.That(processCountAfter == 0, Is.True);
        }
    }
}