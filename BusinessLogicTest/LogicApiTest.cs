using BusinessLogic;

namespace BusinessLogicTest;

[TestClass]
public class LogicApiTest
{
    [TestMethod]
    public void CreateBalls_ShouldCreateCorrectNumberOfBalls()
    {
        LogicAbstractApi logicApi = LogicAbstractApi.CreateApi();

        logicApi.CreateBalls(3, 500, 500);
        var balls = logicApi.GetBalls();

        Assert.AreEqual(3, balls.Count());
    }

    [TestMethod]
    public async Task StartSimulation_ShouldMoveBalls()
    {
        LogicAbstractApi logicApi = LogicAbstractApi.CreateApi();
        logicApi.CreateBalls(1, 500, 500);
        var balls = logicApi.GetBalls().ToList();

        int positionChangesCount = 0;

        balls[0].PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == "X" || args.PropertyName == "Y")
            {
                positionChangesCount++;
            }
        };

        logicApi.StartSimulation();
        await Task.Delay(100);
        logicApi.StopSimulation();

        Assert.IsGreaterThan(0, positionChangesCount);
    }
}
