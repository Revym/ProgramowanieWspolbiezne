using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    internal class BallStatus : IBallStatus
    {
        public double X { get; }
        public double Y { get; }
        public double Radius { get; }

        public BallStatus(IBall ball)
        {
            X = ball.X;
            Y = ball.Y;
            Radius = ball.Radius;
        }
    }
}
