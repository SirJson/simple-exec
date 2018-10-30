using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests
{
    public class CancellingCommands
    {
        [Fact]
        public async Task CancellingARunningCommandStopsCommand()
        {
            var watch = Stopwatch.StartNew();
            using (var cancellationTokenSource = new CancellationTokenSource(50))
            {
                await Command.RunAsync("dotnet", $"exec {Tester.Path} 1000", "", false, cancellationTokenSource.Token);
            }
            watch.Stop();
            Assert.True(watch.Elapsed < TimeSpan.FromMilliseconds(1000), $"Command finished outside window in {watch.Elapsed}");
        }

        [Fact]
        public async Task CancellingAReadingCommandStopsCommand()
        {
            var watch = Stopwatch.StartNew();
            using (var cancellationTokenSource = new CancellationTokenSource(50))
            {
                var result = await Command.ReadAsync("dotnet", $"exec {Tester.Path} 1000", "", false, cancellationTokenSource.Token);
                Assert.Equal("", result);
            }
            watch.Stop();
            Assert.True(watch.Elapsed < TimeSpan.FromMilliseconds(1000), $"Command finished outside window in {watch.Elapsed}");
        }
    }
}