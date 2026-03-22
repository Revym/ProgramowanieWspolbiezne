using Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace BusinessLogic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _dataApi;

        private System.Timers.Timer? _timer;

        private readonly Random _random = new Random();

        public LogicApi(DataAbstractApi dataApi)
        {
            _dataApi = dataApi;
        }

        public override void CreateBalls(int count, int boardWidth, int boardHeight)
        {
            _dataApi.CreateBalls(count, boardWidth, boardHeight);
        }

        public override IEnumerable<IBall> GetBalls()
        {
            return _dataApi.GetBalls();
        }

        public override void StartSimulation()
        {
            if (_timer != null) return;

            _timer = new System.Timers.Timer(30);

            _timer.Elapsed += MoveBalls;

            _timer.AutoReset = true;
            _timer.Start();
        }

        public override void StopSimulation()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }

        private void MoveBalls(object? sender, ElapsedEventArgs e)
        {
            int width = _dataApi.BoardWidth;
            int height = _dataApi.BoardHeight;

            foreach (var ball in _dataApi.GetBalls())
            {
                double moveX = (_random.NextDouble() * 6) - 3;
                double moveY = (_random.NextDouble() * 6) - 3;

                double newX = ball.X + moveX;
                double newY = ball.Y + moveY;

                if (newX <= ball.Radius)
                {
                    newX = ball.Radius;
                }
                else if (newX >= width - ball.Radius)
                {
                    newX = width - ball.Radius;
                }

                if (newY <= ball.Radius)
                {
                    newY = ball.Radius;
                }
                else if (newY >= height - ball.Radius)
                {
                    newY = height - ball.Radius;
                }

                ball.X = newX;
                ball.Y = newY;
            }
        }
    }
}
