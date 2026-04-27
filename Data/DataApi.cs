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
            _balls.Clear();
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;

            double defaultRadius = 15.0;

            double minX = defaultRadius;
            double maxX = boardWidth - defaultRadius;

            double minY = defaultRadius;
            double maxY = boardHeight - defaultRadius;

            for (int i = 0; i < count; i++)
            {
                double x = minX + (_random.NextDouble() * (maxX - minX));
                double y = minY + (_random.NextDouble() * (maxY - minY));

                double vx = (_random.NextDouble() * 6.0) - 3.0;
                double vy = (_random.NextDouble() * 6.0) - 3.0;

                if (vx == 0) vx = 1.0;
                if (vy == 0) vy = 1.0;

                Vector2D velocity = new Vector2D(vx, vy);

                _balls.Add(new Ball(x, y, defaultRadius, velocity));
            }
        }

        public override IEnumerable<IBall> GetBalls()
        {
            return _balls;
        }
    }
}
