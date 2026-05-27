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
        public async Task CheckCollisions_BallsCollide_VelocityChanges()
        {
            DataAbstractApi fakeData = new FakeDataApi();
            LogicAbstractApi logicApi = LogicAbstractApi.CreateApi(fakeData);

            logicApi.CreateBalls(2, 500, 500);
            logicApi.StartSimulation();

            var balls = fakeData.GetBalls().ToList();
            var ball1 = (FakeBall)balls[0];
            var ball2 = (FakeBall)balls[1];

            double initialVx1 = ball1.Velocity.X;
            double initialVx2 = ball2.Velocity.X;

            ball1.Move(125, 100);

            await Task.Delay(30);

            Assert.AreNotEqual(initialVx1, ball1.Velocity.X, "Prędkość kuli 1 nie uległa zmianie");
            Assert.AreNotEqual(initialVx2, ball2.Velocity.X, "Prędkość kuli 2 nie uległa zmianie");

            logicApi.StopSimulation();
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