using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;

namespace BusinessLogic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _dataApi;
        private readonly object _collisionLock = new object();

        private readonly BallBinaryTree _kdTree = new BallBinaryTree();

        public override int BoardWidth => _dataApi.BoardWidth;
        public override int BoardHeight => _dataApi.BoardHeight;

        public override event EventHandler<IEnumerable<IBallStatus>>? SimulationUpdated;

        private CancellationTokenSource? _broadcastCts;
        private Task? _broadcastTask;
        private readonly int _fpsIntervalMs = 16; 

        public LogicApi(DataAbstractApi dataApi)
        {
            _dataApi = dataApi;
        }

        public override void CreateBalls(int count, int boardWidth, int boardHeight)
        {
            _dataApi.CreateBalls(count, boardWidth, boardHeight);

            foreach (var ball in _dataApi.GetBalls())
            {
                ball.PropertyChanged += OnBallMoved;
            }
        }

        public override IEnumerable<IBallStatus> GetBallsStatus()
        {
            return _dataApi.GetBalls().Select(b => new BallStatus(b));
        }

        public override void StartSimulation()
        {
            //SimulationUpdated?.Invoke(this, GetBallsStatus().ToList());
            _broadcastCts = new CancellationTokenSource();

            _broadcastTask = Task.Run(async () =>
            {
                while (!_broadcastCts.Token.IsCancellationRequested)
                {
                    var ballsSnapshot = GetBallsStatus().ToList();
                    SimulationUpdated?.Invoke(this, ballsSnapshot);

                    try
                    {
                        await Task.Delay(_fpsIntervalMs, _broadcastCts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }

                }
            }, _broadcastCts.Token);
        }

        public override void StopSimulation()
        {
            if (_broadcastCts != null)
            {
                _broadcastCts.Cancel();
                try
                {
                    _broadcastTask?.Wait();
                }
                catch (AggregateException) { }

                _broadcastCts.Dispose();
                _broadcastCts = null;
                _broadcastTask = null;
            }
            
            foreach (var ball in _dataApi.GetBalls())
            {
                ball.PropertyChanged -= OnBallMoved;
            }
        }

        private void OnBallMoved(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is IBall ball && (e.PropertyName == "X" || e.PropertyName == "Y"))
            {
                CheckCollisions(ball);
            }
        }

        private void CheckCollisions(IBall currentBall)
        {
            lock (_collisionLock)
            {
                var allBalls = _dataApi.GetBalls().ToList();

                HandleWallCollisions(currentBall);

                _kdTree.Build(allBalls);

                double searchArea = currentBall.Radius + 25.0;

                var nearbyBalls = _kdTree.FindPotentialCollisions(currentBall, searchArea);

                foreach (var otherBall in nearbyBalls)
                {
                    double xDiff = currentBall.X - otherBall.X;
                    double yDiff = currentBall.Y - otherBall.Y;
                    double distanceSquared = xDiff * xDiff + yDiff * yDiff;
                    double distance = Math.Sqrt(distanceSquared);

                    double minDistance = currentBall.Radius + otherBall.Radius;

                    if (distance <= minDistance)
                    {
                        double vxDiff = currentBall.Velocity.X - otherBall.Velocity.X;
                        double vyDiff = currentBall.Velocity.Y - otherBall.Velocity.Y;
                        double dotProduct = vxDiff * xDiff + vyDiff * yDiff;

                        if (dotProduct < 0)
                        {
                            double m1 = currentBall.Mass;
                            double m2 = otherBall.Mass;

                            double scalar1 = (2 * m2 / (m1 + m2)) * (dotProduct / distanceSquared);
                            double scalar2 = (2 * m1 / (m1 + m2)) * (dotProduct / distanceSquared);

                            double newVx1 = currentBall.Velocity.X - scalar1 * xDiff;
                            double newVy1 = currentBall.Velocity.Y - scalar1 * yDiff;

                            double newVx2 = otherBall.Velocity.X - scalar2 * (-xDiff);
                            double newVy2 = otherBall.Velocity.Y - scalar2 * (-yDiff);


                            currentBall.Velocity = new Vector2D(newVx1, newVy1);
                            otherBall.Velocity = new Vector2D(newVx2, newVy2);
                        }
                    }
                }
            }
        }

        private void HandleWallCollisions(IBall currentBall)
        {
            int width = BoardWidth;
            int height = BoardHeight;

            if (currentBall.X <= currentBall.Radius && currentBall.Velocity.X < 0)
            {
                currentBall.X = currentBall.Radius;
                currentBall.Velocity = new Vector2D(-currentBall.Velocity.X, currentBall.Velocity.Y);
            }
            else if (currentBall.X >= width - currentBall.Radius && currentBall.Velocity.X > 0)
            {
                currentBall.X = width - currentBall.Radius;
                currentBall.Velocity = new Vector2D(-currentBall.Velocity.X, currentBall.Velocity.Y);
            }

            if (currentBall.Y <= currentBall.Radius && currentBall.Velocity.Y < 0)
            {
                currentBall.Y = currentBall.Radius;
                currentBall.Velocity = new Vector2D(currentBall.Velocity.X, -currentBall.Velocity.Y);
            }
            else if (currentBall.Y >= height - currentBall.Radius && currentBall.Velocity.Y > 0)
            {
                currentBall.Y = height - currentBall.Radius;
                currentBall.Velocity = new Vector2D(currentBall.Velocity.X, -currentBall.Velocity.Y);
            }
        }
    }
}