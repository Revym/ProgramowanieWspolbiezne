using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    internal class DiagnosticLogger : IDisposable
    {
        private readonly BlockingCollection<string> _logBuffer = new BlockingCollection<string>(100);
        private readonly Task _loggingTask;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly string _filePath;

        public DiagnosticLogger()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "diagnostics.log");
            File.WriteAllText(_filePath, string.Empty);

            _loggingTask = Task.Run(WriteLogsToFile);
        }

        public void LogBallState(IBall ball)
        {
            if (_cts.IsCancellationRequested) return;

            var logEntry = new
            {
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                BallId = ball.GetHashCode(),
                X = Math.Round(ball.X, 2),
                Y = Math.Round(ball.Y, 2),
                VelocityX = Math.Round(ball.Velocity.X, 2),
                VelocityY = Math.Round(ball.Velocity.Y, 2)
            };

            string jsonString = JsonSerializer.Serialize(logEntry);

            _logBuffer.TryAdd(jsonString);
        }

        private void WriteLogsToFile()
        {
            try
            {
                foreach (var log in _logBuffer.GetConsumingEnumerable(_cts.Token))
                {
                    File.AppendAllText(_filePath, log + Environment.NewLine, Encoding.ASCII);
                }
            }
            catch (OperationCanceledException) { }
        }

        public void Dispose()
        {
            _logBuffer.CompleteAdding();
            _cts.Cancel();
            _loggingTask.Wait();
            _logBuffer.Dispose();
            _cts.Dispose();
        }
    }
}