using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    internal class DataApi : DataAbstractApi
    {
        private readonly List<IBall> _balls = new List<IBall>();

        private readonly Random _random = new Random();

        private int _boardWidth;
        private int _boardHeight;

        public override int BoardWidth => _boardWidth;
        public override int BoardHeight => _boardHeight;

        public override void CreateBalls(int count, int boardWidth, int boardHeight)
        {
            foreach (var ball in _balls)
            {
                if (ball is IDisposable disposableBall) disposableBall.Dispose();
            }

            _balls.Clear();
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;

            for (int i = 0; i < count; i++)
            {
                double radius = 10.0 + (_random.NextDouble() * 10.0);
                double mass = Math.PI * radius * radius;

                double minX = radius;
                double maxX = boardWidth - radius;

                double minY = radius;
                double maxY = boardHeight - radius;

                double x = minX + (_random.NextDouble() * (maxX - minX));
                double y = minY + (_random.NextDouble() * (maxY - minY));

                double vx = (_random.NextDouble() * 6.0) - 3.0;
                double vy = (_random.NextDouble() * 6.0) - 3.0;

                if (vx == 0) vx = 1.0;
                if (vy == 0) vy = 1.0;

                Vector2D velocity = new Vector2D(vx, vy);

                _balls.Add(new Ball(x, y, radius, velocity, mass));
            }
        }

        public override IEnumerable<IBall> GetBalls()
        {
            return _balls;
        }
    }
}
