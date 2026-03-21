using Data;

namespace DataTest
{
    [TestClass]
    public sealed class DataApiTest
    {
        [TestMethod]
        public void CreateApi_ShouldReturnInstance()
        {
            DataAbstractApi api = DataAbstractApi.CreateApi();

            Assert.IsNotNull(api);
        }

        [TestMethod]
        public void CreateBalls_ShouldCreateExactNumberOfBalls()
        {
            DataAbstractApi api = DataAbstractApi.CreateApi();
            int ballsCount = 5;

            api.CreateBalls(ballsCount, 400, 400);
            var balls = api.GetBalls();

            Assert.AreEqual(ballsCount, balls.Count());
        }

        [TestMethod]
        public void CreateBalls_BallsShouldStayWithinBoardBoundaries()
        {
            DataAbstractApi api = DataAbstractApi.CreateApi();
            int boardWidth = 400;
            int boardHeight = 400;
            double expectedRadius = 15.0;

            api.CreateBalls(10, boardWidth, boardHeight);
            var balls = api.GetBalls();

            foreach (var ball in balls)
            {
                Assert.AreEqual(expectedRadius, ball.Radius);

                Assert.IsGreaterThanOrEqualTo(expectedRadius, ball.X);
                Assert.IsLessThanOrEqualTo(boardWidth - expectedRadius, ball.X);

                Assert.IsGreaterThanOrEqualTo(expectedRadius, ball.Y);
                Assert.IsLessThanOrEqualTo(boardHeight - expectedRadius, ball.Y);
            }
        }
    }
}
