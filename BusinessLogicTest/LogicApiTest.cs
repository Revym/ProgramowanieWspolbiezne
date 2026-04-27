using BusinessLogic;
using Data;

namespace BusinessLogicTest;

[TestClass]
public class LogicApiTest
{
    [TestMethod]
    public void CreateBalls_ShouldCreateCorrectNumberOfBalls()
    {
        DataAbstractApi fakeData = new FakeDataApi();
        LogicAbstractApi logicApi = LogicAbstractApi.CreateApi(fakeData);

        logicApi.CreateBalls(3, 500, 500);
        var balls = logicApi.GetBalls();

        Assert.AreEqual(3, balls.Count());
    }

    [TestMethod]
    public async Task StartSimulation_ShouldMoveBalls()
    {
        DataAbstractApi fakeData = new FakeDataApi();
        LogicAbstractApi logicApi = LogicAbstractApi.CreateApi(fakeData);

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
