using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    internal class Ball : IBall
    {
        private double _x;
        private double _y;
        private readonly double _radius;
        private readonly double _mass;

        private Task? _movementTask;
        private CancellationTokenSource _cancellationTokenSource;
        public Vector2D Velocity { get; set; }
        public double Mass => _mass;
        public double Radius => _radius;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Ball(double x, double y, double radius, Vector2D velocity, double mass = 10)
        {
            _x = x;
            _y = y;
            _radius = radius;
            Velocity = velocity;
            _mass = mass;

            _cancellationTokenSource = new CancellationTokenSource();

            StartMoving();
        }

        public double X
        {
            get => _x;
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged();
                }
            }
        }

        private void StartMoving()
        {
            _movementTask = Task.Run(async () =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();


                try
                {
                    while (!_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        double deltaTime = stopwatch.Elapsed.TotalSeconds;
                        stopwatch.Restart();
                        
                        X += Velocity.X * deltaTime;
                        Y += Velocity.Y * deltaTime;

                        await Task.Delay(16, _cancellationTokenSource.Token);
                    }
                }
                catch (TaskCanceledException) { }

            }, _cancellationTokenSource.Token);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _movementTask?.Wait();
            _cancellationTokenSource.Dispose();
        }
    }
}
