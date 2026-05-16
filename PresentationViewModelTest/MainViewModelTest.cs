using BusinessLogic;
using PresentationModel;
using PresentationViewModel;

namespace PresentationViewModelTest;

[TestClass]
public class MainViewModelTest
{
    internal class FakeBallStatus : IBallStatus
    {
        public double X { get; set; } = 50;
        public double Y { get; set; } = 50;
        public double Radius { get; set; } = 15;
        public double Mass { get; set; } = 15;
    }

    internal class FakeModelApi : ModelAbstractApi
    {
        public override int BoardWidth => 600;
        public override int BoardHeight => 400;

        public override event EventHandler<IEnumerable<IBallStatus>>? ModelUpdated;

        public override void Start(int count)
        {
            var fakeStatuses = new List<IBallStatus>();
            for (int i = 0; i < count; i++)
            {
                fakeStatuses.Add(new FakeBallStatus());
            }

            ModelUpdated?.Invoke(this, fakeStatuses);
        }

        public override void Stop() { }
    }

    [TestMethod]
    public void MainViewModel_StartCommand_ShouldCreateBalls()
    {
        FakeModelApi fakeModel = new FakeModelApi();
        MainViewModel vm = new MainViewModel(fakeModel);

        vm.BallsCountText = "7";
        vm.StartCommand.Execute(null);

        Assert.AreEqual(7, vm.Balls.Count, "Lista nie zapełniła się 7 kulami!");
    }
}