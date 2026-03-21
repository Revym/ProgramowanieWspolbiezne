using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public abstract class DataAbstractApi
    {
        public abstract void CreateBalls(int count, int boardWidth, int boardHeight);
        public abstract IEnumerable<IBall> GetBalls();

        public abstract int BoardWidth { get; }
        public abstract int BoardHeight { get; }

        public static DataAbstractApi CreateApi()
        {
            return new DataApi();
        }
    }
}
