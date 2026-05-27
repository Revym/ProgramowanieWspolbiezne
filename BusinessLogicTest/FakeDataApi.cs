using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicTest
{
    internal class FakeDataApi : DataAbstractApi
    {
        private readonly List<IBall> _balls = new List<IBall>();


        private int _boardWidth;
        private int _boardHeight;

        public override int BoardWidth => _boardWidth;
        public override int BoardHeight => _boardHeight;


        public override void CreateBalls(int count, int boardWidth, int boardHeight)
        {
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            _balls.Clear();
            _balls.Add(new FakeBall(100, 100, 20, 100, new Vector2D(50, 0)));
            _balls.Add(new FakeBall(140, 100, 20, 100, new Vector2D(-50, 0)));
        }

        public override IEnumerable<IBall> GetBalls()
        {
            return _balls;
        }

        public override void LogData()
        {
            // Empty
        }
    }
}
