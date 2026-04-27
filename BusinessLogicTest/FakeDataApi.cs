using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicTest
{
    internal class FakeDataApi : DataAbstractApi
    {
        private readonly List<IBall> _balls = new List<IBall>();

        public override int BoardWidth => 500;
        public override int BoardHeight => 500;

        public override void CreateBalls(int count, int boardWidth, int boardHeight)
        {
            _balls.Clear();
            for (int i = 0; i < count; i++)
            {
                _balls.Add(new FakeBall(100.0, 100.0));
            }
        }

        public override IEnumerable<IBall> GetBalls()
        {
            return _balls;
        }
    }
}
