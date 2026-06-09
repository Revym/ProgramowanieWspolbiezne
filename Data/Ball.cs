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

        private Timer? _movementTimer;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private CancellationTokenSource _cancellationTokenSource;

        private Vector2D _velocity;
        private readonly object _velocityLock = new object();

        public Vector2D Velocity
        {
            get
            {
                lock (_velocityLock)
                {
                    return _velocity;
                }
            }
            set
            {
                lock (_velocityLock)
                {
                    _velocity = value;
                }
            }
        }
        public double Mass => _mass;
        public double Radius => _radius;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Ball(double x, double y, double radius, Vector2D velocity, double mass = 10)
        {
            _x = x;
            _y = y;
            _radius = radius;
            _velocity = velocity;
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
            _stopwatch.Start();
            _movementTimer = new Timer(OnTimerTick, null, 0, 16);
        }

        private void OnTimerTick(object? state)
        {
            if (_cancellationTokenSource.IsCancellationRequested) return;

            double deltaTime = _stopwatch.Elapsed.TotalSeconds;
            _stopwatch.Restart();

            Vector2D currentVelocity = this.Velocity;

            X += currentVelocity.X * deltaTime;
            Y += currentVelocity.Y * deltaTime;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _movementTimer?.Dispose();
            _cancellationTokenSource.Dispose();
        }
    }
}
