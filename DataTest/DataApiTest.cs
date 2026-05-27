using Data;

namespace DataTest
{
    [TestClass]
    [DoNotParallelize]
    public sealed class DataApiTest
    {
        [TestMethod]
        public void CreateApi_ShouldReturnInstance()
        {
            DataAbstractApi api = DataAbstractApi.CreateApi();
            Assert.IsNotNull(api);

            if (api is IDisposable disposableApi) disposableApi.Dispose();
        }

        [TestMethod]
        public void CreateBalls_ShouldCreateExactNumberOfBalls()
        {
            DataAbstractApi api = DataAbstractApi.CreateApi();
            int ballsCount = 5;

            api.CreateBalls(ballsCount, 400, 400);
            var balls = api.GetBalls();

            Assert.AreEqual(ballsCount, balls.Count());

            if (api is IDisposable disposableApi) disposableApi.Dispose();
        }

        [TestMethod]
        public void CreateBalls_BallsShouldStayWithinBoardBoundaries()
        {
            DataAbstractApi api = DataAbstractApi.CreateApi();
            int boardWidth = 400;
            int boardHeight = 400;
            double expectedMinRadius = 10.0;

            api.CreateBalls(10, boardWidth, boardHeight);
            var balls = api.GetBalls();

            foreach (var ball in balls)
            {
                Assert.IsTrue(ball.X >= expectedMinRadius && ball.X <= boardWidth - expectedMinRadius, "Kula poza osią X");
                Assert.IsTrue(ball.Y >= expectedMinRadius && ball.Y <= boardHeight - expectedMinRadius, "Kula poza osią Y");
            }

            if (api is IDisposable disposableApi) disposableApi.Dispose();
        }

        [TestMethod]
        public void Logger_LogData_ExecutesWithoutExceptions()
        {
            DataAbstractApi api = DataAbstractApi.CreateApi();
            api.CreateBalls(3, 400, 400);

            try
            {
                api.LogData();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Proces logowania zgłosił krytyczny wyjątek przerwania: {ex.Message}");
            }
            finally
            {
                if (api is IDisposable disposableApi) disposableApi.Dispose();
            }
        }
    }
}
