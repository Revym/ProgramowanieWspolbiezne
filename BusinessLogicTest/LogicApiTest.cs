using BusinessLogic;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicTest
{
    [TestClass]
    public class LogicApiTest
    {
        [TestMethod]
        public void CreateBalls_ShouldCreateCorrectNumberOfBalls()
        {
            DataAbstractApi fakeData = new FakeDataApi();
            LogicAbstractApi logicApi = LogicAbstractApi.CreateApi(fakeData);

            logicApi.CreateBalls(3, 500, 500);
            var balls = logicApi.GetBallsStatus();

            Assert.AreEqual(3, balls.Count());
        }

        [TestMethod]
        public async Task StartSimulation_ShouldTriggerSimulationUpdatedEvent()
        {
            DataAbstractApi fakeData = new FakeDataApi();
            LogicAbstractApi logicApi = LogicAbstractApi.CreateApi(fakeData);

            logicApi.CreateBalls(1, 500, 500);

            int updatesCount = 0;

            logicApi.SimulationUpdated += (sender, args) =>
            {
                updatesCount++;
            };

            logicApi.StartSimulation();

            await Task.Delay(150);

            logicApi.StopSimulation();

            Assert.IsTrue(updatesCount > 0, "Symulacja nie wypchnęła żadnych aktualizacji (SimulationUpdated).");
        }
    }
}